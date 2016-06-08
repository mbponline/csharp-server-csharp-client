using System.Collections.Generic;

namespace Server.Models.Utils.DAL.Common
{

    public class DataViewRemoteEntity<T> : DataViewDto
        where T : class, IEntity
    {
        public DataViewRemoteEntity(DataAdapter dataAdapter, DataContext dataContext)
            : base(dataAdapter)
        {
            this.entityTypeName = typeof(T).Name;
            this.dataContext = dataContext;
        }

        private readonly string entityTypeName;
        private readonly DataContext dataContext;

        public int Count(QueryObject queryObject)
        {
            return this.Count(this.entityTypeName, queryObject);
        }

        public IEnumerable<T> GetItems(QueryObject queryObject)
        {
            var dataDto = base.GetItems(this.entityTypeName, queryObject);
            var result = this.dataContext.AttachEntities<T>(dataDto);
            return result;
        }

        public T GetSingleItem(Dto partialEntity, string[] expand = null)
        {
            var dataDto = base.GetSingleItem(this.entityTypeName, partialEntity, expand);
            var result = this.dataContext.AttachSingleEntitiy<T>(dataDto);
            return result;
        }

        public IEnumerable<T> GetMultipleItems(Dto[] partialEntities, string[] expand = null)
        {
            var dataDto = base.GetMultipleItems(this.entityTypeName, partialEntities, expand);
            var result = this.dataContext.AttachEntities<T>(dataDto);
            return result;
        }

        public T InsertItem(Dto entity)
        {
            var dataDto = base.InsertItem(this.entityTypeName, entity);
            var result = this.dataContext.AttachSingleEntitiy<T>(dataDto);
            return result;
        }

        public IEnumerable<T> InsertItems(Dto[] entities)
        {
            var result = new List<T>();
            var resultSingleSerialDataList = base.InsertItems(this.entityTypeName, entities);
            foreach (var dataDto in resultSingleSerialDataList)
            {
                var data = this.dataContext.AttachSingleEntitiy<T>(dataDto);
                result.Add(data);
            }
            return result;
        }

        public T UpdateItem(Dto partialEntity)
        {
            var dataDto = base.UpdateItem(this.entityTypeName, partialEntity);
            var result = this.dataContext.AttachSingleEntitiy<T>(dataDto);
            return result;
        }

        public IEnumerable<T> UpdateItems(Dto[] partialEntities)
        {
            var result = new List<T>();
            var resultSingleSerialDataList = base.UpdateItems(this.entityTypeName, partialEntities);
            foreach (var dataDto in resultSingleSerialDataList)
            {
                var data = this.dataContext.AttachSingleEntitiy<T>(dataDto);
                result.Add(data);
            }
            return result;
        }

        public T DeleteItem(Dto partialEntity)
        {
            var dataDto = base.DeleteItem(this.entityTypeName, partialEntity);
            var entity = this.dataContext.entitySets[this.entityTypeName].FindByKey((IEntity)partialEntity);
            if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
            {
                this.dataContext.entitySets[this.entityTypeName].DeleteEntity(entity);
            }
            return (T)entity;
        }

        public IEnumerable<T> DeleteItems(Dto[] partialEntities)
        {
            var dataDto = base.DeleteItems(this.entityTypeName, partialEntities);
            var entities = new List<T>();
            if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
            {
                foreach (var partialEntity in partialEntities)
                {
                    var entity = this.dataContext.entitySets[this.entityTypeName].FindByKey((IEntity)partialEntity);
                    entities.Add((T)entity);
                    this.dataContext.entitySets[this.entityTypeName].DeleteEntity(entity);
                }
            }
            return entities;
        }
    }

}