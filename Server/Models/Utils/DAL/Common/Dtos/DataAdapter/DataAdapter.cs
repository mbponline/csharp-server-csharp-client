using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models.Utils.DAL.Common
{
    public class DataAdapter
    {
        public DataAdapter(Metadata metadata, string connectionString)
        {
            this.metadata = metadata;
            this.dialect = metadata.Dialect();
            this.connectionString = connectionString;
        }

        private readonly Metadata metadata;
        private readonly Dialect dialect;
        private readonly string connectionString;

        /**
         * Count entity collection
         */
        public int Count(string entityTypeName, QueryObject queryObject)
        {
            var queryObjectLocal = new QueryObject()
            {
                Filter = queryObject.Filter,
                FilterExpand = queryObject.FilterExpand,
                CustomQueryTable = queryObject.CustomQueryTable
            };
            return ReadUtils.Count(entityTypeName, queryObjectLocal, this.metadata, this.dialect, this.connectionString);
        }

        /**
         * Query entity collection
         */
        public ResultSerialData QueryAll(string entityTypeName, QueryObject queryObject)
        {
            return ReadUtils.Fetch(entityTypeName, queryObject, this.metadata, this.dialect, this.connectionString);
        }

        /**
         * Retrive a single entity
         */
        public ResultSingleSerialData LoadOne(string entityTypeName, Dto dto, string[] expand)
        {
            var resultSerialData = ReadUtils.FetchOne(entityTypeName, dto, expand, this.metadata, this.dialect, this.connectionString);
            if (resultSerialData.Items.Count() > 0)
            {
                return resultSerialData.ToSingle();
            }
            else
            {
                throw new HttpException(httpCode: 404, message: "Not Found");
            }
        }

        /**
         * Retrive multiple entities
         */
        public ResultSerialData LoadMany(string entityTypeName, Dto[] dtos, string[] expand)
        {
            return ReadUtils.FetchMany(entityTypeName, dtos, expand, this.metadata, this.dialect, this.connectionString);
        }

        /**
         * Insert single entity
         */
        public ResultSingleSerialData PostItem(string entityTypeName, Dto dto)
        {
            ResultSerialData resultSerialDataOriginal = null;
            if (CudUtils.KeyPresent(entityTypeName, dto, this.metadata))
            {
                resultSerialDataOriginal = ReadUtils.FetchOne(entityTypeName, dto, null, this.metadata, this.dialect, this.connectionString);
            }
            if (resultSerialDataOriginal != null && resultSerialDataOriginal.Items.Count() > 0)
            {
                throw new HttpException(httpCode: 409, message: "Conflict");
            }
            else
            {
                var resultSerialData = CudUtils.InsertEntity(entityTypeName, dto, this.metadata, this.dialect, this.connectionString);
                return resultSerialData.ToSingle();
            }
        }

        /**
        * Insert multiple entities
        */
        public List<ResultSingleSerialData> PostItems(string entityTypeName, Dto[] dtos)
        {
            ResultSerialData resultSerialDataOriginal = null;
            if (CudUtils.KeysPresent(entityTypeName, dtos, this.metadata))
            {
                resultSerialDataOriginal = ReadUtils.FetchMany(entityTypeName, dtos, null, this.metadata, this.dialect, this.connectionString);
            }
            if (resultSerialDataOriginal != null && resultSerialDataOriginal.Items.Count() > 0)
            {
                throw new HttpException(httpCode: 409, message: "Conflict");
            }
            else
            {
                var entityInserts = new List<ResultSingleSerialData>();
                foreach (var dto in dtos)
                {
                    var resultSerialData = CudUtils.InsertEntity(entityTypeName, dto, this.metadata, this.dialect, this.connectionString);
                    entityInserts.Add(resultSerialData.ToSingle());
                }
                return entityInserts;
            }
        }

        /**
         * Update single entity
         */
        public ResultSingleSerialData PutItem(string entityTypeName, Dto dto)
        {
            var resultSerialDataOriginal = ReadUtils.FetchOne(entityTypeName, dto, null, this.metadata, this.dialect, this.connectionString);
            if (resultSerialDataOriginal.Items.Count() == 0)
            {
                throw new HttpException(httpCode: 404, message: "Not Found");
            }
            else
            {
                const bool returnUpdated = false;
                var resultSerialData = CudUtils.UpdateEntity(entityTypeName, JObject.FromObject(resultSerialDataOriginal.Items.FirstOrDefault()), dto, this.metadata, this.dialect, this.connectionString, returnUpdated);
                if (returnUpdated)
                {
                    if (resultSerialData.Items.Count() > 0)
                    {
                        return resultSerialData.ToSingle();
                    }
                    else
                    {
                        throw new HttpException(httpCode: 304, message: "Not Modified");
                    }
                }
                else
                {
                    throw new HttpException(httpCode: 204, message: "No Content");
                }
            }
        }

        /**
         * Update multiple entities
         */
        public List<ResultSingleSerialData> PutItems(string entityTypeName, Dto[] dtos)
        {
            var resultSerialDataOriginal = ReadUtils.FetchMany(entityTypeName, dtos, null, this.metadata, this.dialect, this.connectionString);
            if (resultSerialDataOriginal.Items.Count() == 0)
            {
                throw new HttpException(httpCode: 404, message: "Not Found");
            }
            else if (resultSerialDataOriginal.Items.Count() != dtos.Length)
            {
                throw new HttpException(httpCode: 400, message: "Bad Request");
            }
            else
            {
                var keyNames = this.metadata.EntityTypes[entityTypeName].Key;
                var resultSerialDataList = DalUtils.LeftJoin(resultSerialDataOriginal.Items, dtos,
                    (ent, dto) => CudUtils.CompareByKey(JObject.FromObject(ent).ToObject<Dto>(), dto, keyNames),
                    (ent, dto) => CudUtils.UpdateEntity(entityTypeName, JObject.FromObject(ent), dto, this.metadata, this.dialect, this.connectionString)
                );
                var resultSingleSerialDataList = new List<ResultSingleSerialData>();
                foreach (var resultSerialData in resultSerialDataList)
                {
                    if (resultSerialData.Items.Count() > 0)
                    {
                        resultSingleSerialDataList.Add(resultSerialData.ToSingle());
                    }
                }

                if (resultSingleSerialDataList.Count > 0)
                {
                    return resultSingleSerialDataList;
                }
                else
                {
                    var total = 0;
                    foreach (var resultSerialData in resultSerialDataList)
                    {
                        total += resultSerialData.Items.Count();
                    }
                    if (total > 0)
                    {
                        throw new HttpException(httpCode: 204, message: "No Content");
                    }
                    else
                    {
                        throw new HttpException(httpCode: 304, message: "Not Modified");
                    }
                }
            }
        }

        /**
         * Delete single entity
         */
        public ResultSingleSerialData DeleteItem(string entityTypeName, Dto dto)
        {
            var resultSerialDataOriginal = ReadUtils.FetchOne(entityTypeName, dto, null, this.metadata, this.dialect, this.connectionString);
            if (resultSerialDataOriginal.Items.Count() == 0)
            {
                throw new HttpException(httpCode: 404, message: "Not Found");
            }
            else
            {
                CudUtils.DeleteEntity(entityTypeName, dto, this.metadata, this.dialect, this.connectionString);
                return resultSerialDataOriginal.ToSingle();
            }
        }

        /**
         * Delete multiple entities
         */
        public ResultSerialData DeleteItems(string entityTypeName, Dto[] dtos)
        {
            var resultSerialDataOriginal = ReadUtils.FetchMany(entityTypeName, dtos, null, this.metadata, this.dialect, this.connectionString);
            if (resultSerialDataOriginal.Items.Count() == 0)
            {
                throw new HttpException(httpCode: 404, message: "Not Found");
            }
            else
            {
                foreach (var dto in dtos)
                {
                    CudUtils.DeleteEntity(entityTypeName, dto, this.metadata, this.dialect, this.connectionString);
                }
                return resultSerialDataOriginal;
            }
        }
    }

}