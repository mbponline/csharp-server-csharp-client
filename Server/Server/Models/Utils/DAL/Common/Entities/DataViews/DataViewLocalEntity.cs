using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Models.Utils.DAL.Common
{
    public class DataViewLocalEntity<T>
        where T : class, IEntity
    {
        public DataViewLocalEntity(DataContext dataContext)
        {
            this.entityTypeName = typeof(T).Name;
            this.dataContext = dataContext;
        }

        private readonly string entityTypeName;
        private readonly DataContext dataContext;

        public IEnumerable<T> GetItems(Func<T, bool> predicate)
        {
            var result = Enumerable.Empty<T>();
            if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
            {
                var entitySetItems = this.dataContext.entitySets[this.entityTypeName].Items.Select((it) => it as T);
                result = entitySetItems.Where(predicate).ToArray();
            }
            return result;
        }

        public IEnumerable<T> GetMultipleItems(List<T> partialEntities)
        {
            var result = Enumerable.Empty<T>();
            var item = default(T);
            if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
            {
                var items = new List<T>();
                foreach (var partialEntity in partialEntities)
                {
                    var value = this.dataContext.entitySets[this.entityTypeName] as IEntitySet<T>;
                    item = value.FindByKey(partialEntity);
                    if (item != null)
                    {
                        items.Add(item);
                    }
                }
                result = items;
            }
            return result;
        }

        public T GetSingleItem(Func<T, bool> predicate)
        {
            var result = null as T;
            if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
            {
                var entitySetItems = this.dataContext.entitySets[this.entityTypeName].Items.Select((it) => it as T);
                result = entitySetItems.FirstOrDefault(predicate);
            }
            return result;
        }

        public T GetSingleItem(T partialEntity)
        {
            var result = null as T;
            if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
            {
                var value = this.dataContext.entitySets[this.entityTypeName] as IEntitySet<T>;
                result = value.FindByKey(partialEntity /*partialEntity*/);
            }
            return result;
        }

        public T CreateItemDetached()
        {
            var item = this.dataContext.CreateItemDetached<T>(this.entityTypeName);
            return item;
        }

        public void DetachItem(T entity)
        {
            this.dataContext.entitySets[this.entityTypeName].DeleteEntity(entity);
        }

        public void DetachItems(T[] entities)
        {
            foreach (var entity in entities)
            {
                this.dataContext.entitySets[this.entityTypeName].DeleteEntity(entity);
            }
        }

        public void DetachAll()
        {
            this.dataContext.entitySets[this.entityTypeName].DeleteAll();
        }
    }

}