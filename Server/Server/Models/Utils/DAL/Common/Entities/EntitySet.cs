using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Server.Models.Utils.DAL.Common
{
    public class EntitySet<T> : IEntitySet<T>
        where T : class, IEntity
    {
        public EntitySet(Type entityType, Dictionary<string, IEntitySet<IEntity>> entitySets, Metadata metadata)
        {
            this.entityTypeName = entityType.Name;
            this.entitySets = entitySets;
            this.metadata = metadata;
            this.key = metadata.EntityTypes[this.entityTypeName].Key;
            this.localKeyPropertyInfo = entityType.GetProperties().Where(prop => this.key.Contains(prop.Name)).ToArray();
            this.Items = new List<T>();
        }

        private readonly string entityTypeName;
        private Dictionary<string, IEntitySet<IEntity>> entitySets;
        private Metadata metadata;

        private readonly string[] key;
        private readonly PropertyInfo[] localKeyPropertyInfo;

        public List<T> Items { get; private set; }

        //public static IEntitySet<IEntity> CreateEntitySet(Dictionary<string, IEntitySet<IEntity>> entitySets, Metadata metadata)
        //{
        //    var d1 = typeof(EntitySet<>);
        //    var typeArgs = new Type[] { typeof(IEntity) };
        //    var constructed = d1.MakeGenericType(typeArgs);
        //    var entitySet = (IEntitySet<IEntity>)Activator.CreateInstance(constructed, new object[] { entitySets, metadata });
        //    return entitySet;
        //}

        public T NavigateSingle(IEntity remoteEntity, string[] remoteEntityKey, string[] navigationKey)
        {
            var result = this.Items.FirstOrDefault((it) => this.HaveSameKeysNavigation(it as Dto, navigationKey, remoteEntity as Dto, remoteEntityKey));
            return result;
        }

        public IEnumerable<T> NavigateMulti(IEntity remoteEntity, string[] remoteEntityKey, string[] navigationKey)
        {
            var result = this.Items.Where((it) => this.HaveSameKeysNavigation(it as Dto, navigationKey, remoteEntity as Dto, remoteEntityKey));
            return result;
        }

        public IEnumerable<T> NavigateAllRelated(IEnumerable<object> remoteEntities, string[] remoteEntityKey, string[] navigationKey)
        {
            var result = this.Items.Where((it) =>
            {
                foreach (var remoteEntity in remoteEntities)
                {
                    if (this.HaveSameKeysNavigation(it as Dto, navigationKey, (Dto)remoteEntity, remoteEntityKey))
                    {
                        return true;
                    }
                }
                return false;
            });
            return result;
        }

        public T FindByKey(T partialEntity)
        {
            var result = this.Items.FirstOrDefault((it) => this.HaveSameKeysLocal(it as Dto, partialEntity as Dto));
            return result;
        }

        public T Find(Func<T, bool> predicate)
        {
            var result = this.Items.FirstOrDefault(predicate);
            return result;
        }

        public IEnumerable<T> Filter(Func<T, bool> predicate)
        {
            var result = this.Items.Where(predicate);
            return result;
        }

        public void DeleteEntity(T entity)
        {
            entity._detach();
            this.Items.Remove(entity);
        }

        public void DeleteAll()
        {
            foreach (var entity in this.Items)
            {
                entity._detach();
            }
            this.Items.RemoveAll((it) => true);
        }

        public void Dispose()
        {
            this.DeleteAll();
            this.entitySets = null;
            this.metadata = null;
        }

        public T UpdateEntity(T dto)
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
                newItem = this.Initialize(dto as Dto, found as Dto);

            }

            return newItem;
        }

        public void AttachEntitySet(List<T> dtos)
        {
            foreach (var dto in dtos)
            {
                this.CreateNewItem(dto);
            }
            this.Items = dtos as List<T>;
        }

        private T CreateNewItem(T dto)
        {
            dto._attach(this.entitySets, this.metadata);
            return dto;
        }

        private T Initialize(Dto dto, Dto entity)
        {
            foreach (var prop in dto)
            {
                entity[prop.Key] = prop.Value;
            }
            return entity as T;
        }

        private bool HaveSameKeysLocal(Dto localEntity, Dto remoteEntity)
        {
            for (int i = 0; i < this.key.Length; i++)
            {
                if ((int)localEntity[this.key[i]] != (int)remoteEntity[this.key[i]])
                {
                    return false;
                }
            }
            return true;
        }

        private bool HaveSameKeysNavigation(Dto localEntity, string[] keyLocal, Dto remoteEntity, string[] keyRemote)
        {
            for (int i = 0; i < keyLocal.Length; i++)
            {
                if ((int)localEntity[keyLocal[i]] != (int)remoteEntity[keyRemote[i]])
                {
                    return false;
                }
            }
            return true;
        }
    }

}