using System;
using System.Collections.Generic;
using System.Linq;

namespace Client.Modules.Utils.DAL.Common
{
    public class DataViewLocal<T>
        where T : class, IDerivedEntity
    {
        public DataViewLocal(DataContext dataContext)
        {
            this.entityTypeName = typeof(T).Name;
            this.dataContext = dataContext;
        }

        private readonly string entityTypeName;
        private readonly DataContext dataContext;

        public IEnumerable<T> GetItems(Func<T, bool> predicate)
        {
            var derivedEntityList = Enumerable.Empty<T>();
            if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
            {
                var entitySetItems = this.dataContext.entitySets[this.entityTypeName].Items.Select((it) => it as T);
                derivedEntityList = entitySetItems.Where(predicate).ToArray();
            }
            return derivedEntityList;
        }

        public IEnumerable<T> GetMultipleItems(IEnumerable<Dto> partialDtos)
        {
            var derivedEntityList = new List<T>();
            var derivedEntity = default(T);
            if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
            {
                var entitySet = (IEntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
                foreach (var partialDto in partialDtos)
                {
                    derivedEntity = entitySet.FindByKey(partialDto);
                    if (derivedEntity != null)
                    {
                        derivedEntityList.Add(derivedEntity);
                    }
                }
            }
            return derivedEntityList;
        }

        public T GetSingleItem(Func<T, bool> predicate)
        {
            var derivedEntity = default(T);
            if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
            {
                var entitySetItems = this.dataContext.entitySets[this.entityTypeName].Items.Select((it) => it as T);
                derivedEntity = entitySetItems.FirstOrDefault(predicate);
            }
            return derivedEntity;
        }

        public T GetSingleItem(Dto partialDto)
        {
            var derivedEntity = default(T);
            if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
            {
                var entitySet = (IEntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
                derivedEntity = entitySet.FindByKey(partialDto /*partialEntity*/);
            }
            return derivedEntity;
        }

        public T CreateItemDetached()
        {
            var derivedEntity = this.dataContext.CreateItemDetached<T>(this.entityTypeName);
            return derivedEntity;
        }

        public void DetachItem(T derivedEntity)
        {
            var entitySet = (IEntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
            entitySet.DeleteEntity(derivedEntity);
        }

        public void DetachItems(IEnumerable<T> derivedEntityList)
        {
            var entitySet = (IEntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
            foreach (var derivedEntity in derivedEntityList)
            {
                entitySet.DeleteEntity(derivedEntity);
            }
        }

        public void DetachAll()
        {
            var entitySet = (IEntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
            entitySet.DeleteAll();
        }
    }

}