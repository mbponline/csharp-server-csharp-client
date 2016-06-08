using System.Collections.Generic;

namespace Server.Models.Utils.DAL.Common
{

    public class DataViewDto
    {
        public DataViewDto(DataAdapter dataAdapter)
        {
            this.dataAdapter = dataAdapter;
        }

        private readonly DataAdapter dataAdapter;

        public int Count(string entityTypeName, QueryObject queryObject)
        {
            return this.dataAdapter.Count(entityTypeName, queryObject);
        }

        public ResultSerialData GetItems(string entityTypeName, QueryObject queryObject)
        {
            return this.dataAdapter.QueryAll(entityTypeName, queryObject);
        }

        public ResultSingleSerialData GetSingleItem(string entityTypeName, Dto partialEntity, string[] expand = null)
        {
            return this.dataAdapter.LoadOne(entityTypeName, partialEntity, expand);
        }

        public ResultSerialData GetMultipleItems(string entityTypeName, Dto[] partialEntities, string[] expand = null)
        {
            return this.dataAdapter.LoadMany(entityTypeName, partialEntities, expand);
        }

        public ResultSingleSerialData InsertItem(string entityTypeName, Dto entity)
        {
            return this.dataAdapter.PostItem(entityTypeName, entity);
        }

        public List<ResultSingleSerialData> InsertItems(string entityTypeName, Dto[] entities)
        {
            return this.dataAdapter.PostItems(entityTypeName, entities);
        }

        public ResultSingleSerialData UpdateItem(string entityTypeName, Dto partialEntity)
        {
            return this.dataAdapter.PutItem(entityTypeName, partialEntity);
        }

        public List<ResultSingleSerialData> UpdateItems(string entityTypeName, Dto[] partialEntities)
        {
            return this.dataAdapter.PutItems(entityTypeName, partialEntities);
        }

        public ResultSingleSerialData DeleteItem(string entityTypeName, Dto partialEntity)
        {
            return this.dataAdapter.DeleteItem(entityTypeName, partialEntity);
        }

        public ResultSerialData DeleteItems(string entityTypeName, Dto[] partialEntities)
        {
            return this.dataAdapter.DeleteItems(entityTypeName, partialEntities);
        }
    }

}