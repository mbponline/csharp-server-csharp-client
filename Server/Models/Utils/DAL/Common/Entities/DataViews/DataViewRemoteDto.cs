using System.Collections.Generic;

namespace Server.Models.Utils.DAL.Common
{

    public class DataViewRemoteDto<T> : DataViewDto
        where T : class, IEntity
    {
        public DataViewRemoteDto(DataAdapter dataAdapter)
            : base(dataAdapter)
        {
            this.entityTypeName = typeof(T).Name;
            this.dataAdapter = dataAdapter;
        }

        private readonly string entityTypeName;
        private readonly DataAdapter dataAdapter;

        public int Count(QueryObject queryObject)
        {
            return base.Count(this.entityTypeName, queryObject);
        }

        public ResultSerialData GetItems(QueryObject queryObject)
        {
            return base.GetItems(this.entityTypeName, queryObject);
        }

        public ResultSingleSerialData GetSingleItem(Dto partialEntity, string[] expand = null)
        {
            return this.GetSingleItem(this.entityTypeName, partialEntity, expand);
        }

        public ResultSerialData GetMultipleItems(Dto[] partialEntities, string[] expand = null)
        {
            return this.GetMultipleItems(this.entityTypeName, partialEntities, expand);
        }

        public ResultSingleSerialData InsertItem(Dto entity)
        {
            return this.InsertItem(this.entityTypeName, entity);
        }

        public List<ResultSingleSerialData> InsertItems(Dto[] entities)
        {
            return this.InsertItems(this.entityTypeName, entities);
        }

        public ResultSingleSerialData UpdateItem(Dto partialEntity)
        {
            return this.UpdateItem(this.entityTypeName, partialEntity);
        }

        public List<ResultSingleSerialData> UpdateItems(Dto[] partialEntities)
        {
            return this.UpdateItems(this.entityTypeName, partialEntities);
        }

        public ResultSingleSerialData DeleteItem(Dto partialEntity)
        {
            return this.DeleteItem(this.entityTypeName, partialEntity);
        }

        public ResultSerialData DeleteItems(Dto[] partialEntities)
        {
            return this.DeleteItems(this.entityTypeName, partialEntities);
        }
    }

}