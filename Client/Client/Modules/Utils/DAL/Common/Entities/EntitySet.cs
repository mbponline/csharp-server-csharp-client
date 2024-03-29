﻿using System;
using System.Linq;
using System.Collections.Generic;
using MetadataCli = Client.Modules.Utils.DAL.Common.MetadataCli;

namespace Client.Modules.Utils.DAL.Common
{
    public class EntitySet<T> : IEntitySet<T>
        where T : class, IDerivedEntity
    {
        public EntitySet(Type derivedEntityType, Dictionary<string, IEntitySet<IDerivedEntity>> entitySets, MetadataCli.Metadata metadataCli)
        {
            this.derivedEntityType = derivedEntityType;
            this.entityTypeName = derivedEntityType.Name;
            this.entitySets = entitySets;
            this.metadataCli = metadataCli;
            this.key = metadataCli.EntityTypes[this.entityTypeName].Key;
            this.Items = new List<IDerivedEntity>();
        }

        private readonly string entityTypeName;
        private readonly Type derivedEntityType;
        private Dictionary<string, IEntitySet<IDerivedEntity>> entitySets;
        private MetadataCli.Metadata metadataCli;
        private readonly string @namespace;

        private readonly string[] key;

        public List<IDerivedEntity> Items { get; private set; }

        /*
		*/
        public static IEntitySet<IDerivedEntity> CreateEntitySet(string entityTypeName, Dictionary<string, IEntitySet<IDerivedEntity>> entitySets, string @namespace, MetadataCli.Metadata metadataCli)
        {
            var derivedEntityType = Type.GetType(@namespace + "." + entityTypeName);
            var d1 = typeof(EntitySet<>);
            var typeArgs = new Type[] { derivedEntityType };
            var constructed = d1.MakeGenericType(typeArgs);
            var entitySet = Activator.CreateInstance(constructed, new object[] { derivedEntityType, entitySets, metadataCli });
            return (IEntitySet<IDerivedEntity>)entitySet;
        }

        public T NavigateSingle(Entity remoteEntity, string[] remoteEntityKey, string[] navigationKey)
        {
            var derivedEntity = this.Items.FirstOrDefault((it) => this.HaveSameKeysNavigation(it.entity.dto, navigationKey, remoteEntity.dto, remoteEntityKey));
            return (T)derivedEntity;
        }

        public IEnumerable<T> NavigateMulti(Entity remoteEntity, string[] remoteEntityKey, string[] navigationKey)
        {
            var derivedEntityList = this.Items.Where((it) => this.HaveSameKeysNavigation(it.entity.dto, navigationKey, remoteEntity.dto, remoteEntityKey));
            return derivedEntityList.Cast<T>().ToList();
        }

        public IEnumerable<T> NavigateAllRelated(IEnumerable<Dto> remoteDtos, string[] remoteEntityKey, string[] navigationKey)
        {
            var derivedEntityList = this.Items.Where((it) =>
            {
                foreach (var remoteDto in remoteDtos)
                {
                    if (this.HaveSameKeysNavigation(it.entity.dto, navigationKey, remoteDto, remoteEntityKey))
                    {
                        return true;
                    }
                }
                return false;
            });
            return derivedEntityList.Cast<T>().ToList();
        }

        public T FindByKey(Dto partialDto)
        {
            var derivedEntity = this.Items.FirstOrDefault((it) => this.HaveSameKeysLocal(it.entity.dto, partialDto));
            return (T)derivedEntity;
        }

        public T Find(Func<T, bool> predicate)
        {
            var derivedEntity = this.Items.FirstOrDefault(it => predicate((T)it));
            return (T)derivedEntity;
        }

        public IEnumerable<T> Filter(Func<T, bool> predicate)
        {
            var derivedEntityList = this.Items.Where((it => predicate((T)it)));
            return derivedEntityList.Cast<T>().ToList();
        }

        public void DeleteEntity(IDerivedEntity derivedEntity)
        {
            derivedEntity.entity.Detach();
            this.Items.Remove(derivedEntity);
        }

        public void DeleteAll()
        {
            foreach (var derivedEntity in this.Items)
            {
                derivedEntity.entity.Detach();
            }
            this.Items.RemoveAll((it) => true);
        }

        public void Dispose()
        {
            this.DeleteAll();
            this.entitySets = null;
            this.metadataCli = null;
        }

        public T UpdateEntity(Dto dto)
        {
            T newItem;
            // se cauta elementul in colectia existenta
            var found = this.FindByKey(dto);
            if (found == null)
            {
                // daca nu a fost gasit se adauga in colectie
                newItem = this.CreateNewItem(dto);
                this.Items.Add(newItem);
            }
            else
            {
                // daca a fost gasit nu se inlocuieste ci se actualizaeza datale
                // astfel ca astfel ca referintele din dataViews existente sa nu se piarda.
                newItem = this.Initialize(dto, found);

            }

            return newItem;
        }

        public void AttachEntitySet(List<Dto> dtos)
        {
            var derivedEntityList = new List<IDerivedEntity>();
            foreach (var dto in dtos)
            {
                derivedEntityList.Add(this.CreateNewItem(dto));
            }
            this.Items = derivedEntityList;
        }

        private T CreateNewItem(Dto dto)
        {
            var entity = new Entity(this.entityTypeName, dto);
            entity.Attach(this.entitySets, this.metadataCli);
            var derivedEntity = (T)Activator.CreateInstance(this.derivedEntityType, new object[] { entity });
            return derivedEntity;
        }

        private T Initialize(Dto dto, T derivedEntity)
        {
            //foreach (var prop in dto)
            //{
            //	entity[prop.Key] = prop.Value;
            //}

            // Nu este nevoie sa se copieze proprietatile.
            // Toate referintele externe se fac la Entity asadar se poate
            // inlocui referinta la Dto fara a afecta integritatea referentiala

            derivedEntity.entity.dto = dto;
            return derivedEntity;
        }

        private bool HaveSameKeysLocal(Dto localDto, Dto remoteDto)
        {
            for (int i = 0; i < this.key.Length; i++)
            {
                if ((long)localDto[this.key[i]] != (long)remoteDto[this.key[i]])
                {
                    return false;
                }
            }
            return true;
        }

        private bool HaveSameKeysNavigation(Dto localDto, string[] keyLocal, Dto remoteDto, string[] keyRemote)
        {
            for (int i = 0; i < keyLocal.Length; i++)
            {
                if ((long)localDto[keyLocal[i]] != (long)remoteDto[keyRemote[i]])
                {
                    return false;
                }
            }
            return true;
        }
    }

}
