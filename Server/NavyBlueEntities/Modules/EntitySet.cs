using NavyBlueDtos;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MetadataSrv = NavyBlueDtos.MetadataSrv;

namespace NavyBlueEntities
{
    public class EntitySet<T> : IEntitySet<T>
        where T : class, IDerivedEntity
    {
        private readonly string entityTypeName;
        private readonly Type derivedEntityType;
        private Dictionary<string, IEntitySet<IDerivedEntity>> entitySets;
        private MetadataSrv.Metadata metadataSrv;
        private readonly string[] key;
        public List<IDerivedEntity> Items { get; private set; }

        public EntitySet(Type derivedEntityType, Dictionary<string, IEntitySet<IDerivedEntity>> entitySets, MetadataSrv.Metadata metadataSrv)
        {
            this.derivedEntityType = derivedEntityType;
            this.entityTypeName = derivedEntityType.Name;
            this.entitySets = entitySets;
            this.metadataSrv = metadataSrv;
            this.key = metadataSrv.EntityTypes[this.entityTypeName].Key;
            this.Items = new List<IDerivedEntity>();
        }

        public static IEntitySet<IDerivedEntity> CreateEntitySet(string entityTypeName, Dictionary<string, IEntitySet<IDerivedEntity>> entitySets, MetadataSrv.Metadata metadataSrv)
        {
            var currentAssemblyName = Assembly.GetExecutingAssembly().CodeBase; //.Location
            var currentAssemblyUri = new Uri(currentAssemblyName);
            var path = Path.GetDirectoryName(currentAssemblyUri.LocalPath);
            var targetAssemblyName = Path.Combine(path, string.Format("{0}.dll", metadataSrv.Namespace.Split(new char[] { '.' }).FirstOrDefault()));
            var targetAssembly = Assembly.LoadFrom(targetAssemblyName);
            var derivedEntityType = targetAssembly.GetType(metadataSrv.Namespace + "." + entityTypeName);
            //var derivedEntityType = Type.GetType(metadataSrv.Namespace + "." + entityTypeName);
            var d1 = typeof(EntitySet<>);
            var typeArgs = new Type[] { derivedEntityType };
            var constructed = d1.MakeGenericType(typeArgs);
            var entitySet = Activator.CreateInstance(constructed, new object[] { derivedEntityType, entitySets, metadataSrv });
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
            this.metadataSrv = null;
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
            entity.Attach(this.entitySets, this.metadataSrv);
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
                if (!localDto[this.key[i]].Equals(remoteDto[this.key[i]]))
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
                if (!localDto[keyLocal[i]].Equals(remoteDto[keyRemote[i]]))
                {
                    return false;
                }
            }
            return true;
        }
    }

}
