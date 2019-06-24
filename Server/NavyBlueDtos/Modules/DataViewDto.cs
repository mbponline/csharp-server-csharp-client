using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace NavyBlueDtos
{
    public class DataViewDto
    {
        private readonly DataAdapterRead dataAdapterRead;
        private readonly DataAdapterCud dataAdapterCud;
        private readonly MetadataSrv.Metadata metadataSrv;

        internal DataViewDto(DataAdapterRead dataAdapterRead, DataAdapterCud dataAdapterCud, MetadataSrv.Metadata metadataSrv)
        {
            this.dataAdapterRead = dataAdapterRead;
            this.dataAdapterCud = dataAdapterCud;
            this.metadataSrv = metadataSrv;
        }

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
            return this.dataAdapterRead.Count(entityTypeName, queryObjectLocal);
        }

        /**
         * Query entity collection
         */
        public ResultSerialData GetItems(string entityTypeName, QueryObject queryObject)
        {
            return this.dataAdapterRead.Fetch(entityTypeName, queryObject);
        }

        /**
         * Retrive a single entity
         */
        public ResultSingleSerialData GetSingleItem(string entityTypeName, Dto dto, string[] expand)
        {
            var resultSerialData = this.dataAdapterRead.FetchOne(entityTypeName, dto, expand);
            if (resultSerialData.Items.Count() > 0)
            {
                return resultSerialData.ToSingle();
            }
            else
            {
                throw new DtosException(code: 404, message: "Not Found");
            }
        }

        /**
         * Retrive multiple entities
         */
        public ResultSerialData GetMultipleItems(string entityTypeName, IEnumerable<Dto> dtos, string[] expand)
        {
            return this.dataAdapterRead.FetchMany(entityTypeName, dtos, expand);
        }

        /**
         * Insert single entity
         */
        public ResultSingleSerialData InsertItem(string entityTypeName, Dto dto)
        {
            ResultSerialData resultSerialDataOriginal = null;
            if (Utils.KeyPresent(entityTypeName, dto, this.metadataSrv))
            {
                resultSerialDataOriginal = this.dataAdapterRead.FetchOne(entityTypeName, dto, null);
            }
            if (resultSerialDataOriginal != null && resultSerialDataOriginal.Items.Count() > 0)
            {
                throw new DtosException(code: 409, message: "Conflict");
            }
            else
            {
                var resultSerialData = this.dataAdapterCud.InsertEntity(entityTypeName, dto);
                return resultSerialData.ToSingle();
            }
        }

        /**
        * Insert multiple entities
        */
        public List<ResultSingleSerialData> InsertItems(string entityTypeName, IEnumerable<Dto> dtos)
        {
            ResultSerialData resultSerialDataOriginal = null;
            if (Utils.KeysPresent(entityTypeName, dtos, this.metadataSrv))
            {
                resultSerialDataOriginal = this.dataAdapterRead.FetchMany(entityTypeName, dtos, null);
            }
            if (resultSerialDataOriginal != null && resultSerialDataOriginal.Items.Count() > 0)
            {
                throw new DtosException(code: 409, message: "Conflict");
            }
            else
            {
                var entityInserts = new List<ResultSingleSerialData>();
                foreach (var dto in dtos)
                {
                    var resultSerialData = this.dataAdapterCud.InsertEntity(entityTypeName, dto);
                    entityInserts.Add(resultSerialData.ToSingle());
                }
                return entityInserts;
            }
        }

        /**
         * Update single entity
         */
        public ResultSingleSerialData UpdateItem(string entityTypeName, Dto dto)
        {
            var resultSerialDataOriginal = this.dataAdapterRead.FetchOne(entityTypeName, dto, null);
            if (resultSerialDataOriginal.Items.Count() == 0)
            {
                throw new DtosException(code: 404, message: "Not Found");
            }
            else
            {
                const bool returnUpdated = false;
                var resultSerialData = this.dataAdapterCud.UpdateEntity(entityTypeName, JObject.FromObject(resultSerialDataOriginal.Items.FirstOrDefault()), dto);
                if (returnUpdated)
                {
                    if (resultSerialData.Items.Count() > 0)
                    {
                        return resultSerialData.ToSingle();
                    }
                    else
                    {
                        throw new DtosException(code: 304, message: "Not Modified");
                    }
                }
                else
                {
                    throw new DtosException(code: 204, message: "No Content");
                }
            }
        }

        /**
         * Update multiple entities
         */
        public List<ResultSingleSerialData> UpdateItems(string entityTypeName, IEnumerable<Dto> dtos)
        {
            var resultSerialDataOriginal = this.dataAdapterRead.FetchMany(entityTypeName, dtos, null);
            if (resultSerialDataOriginal.Items.Count() == 0)
            {
                throw new DtosException(code: 404, message: "Not Found");
            }
            else if (resultSerialDataOriginal.Items.Count() != dtos.Count())
            {
                throw new DtosException(code: 400, message: "Bad Request");
            }
            else
            {
                var keyNames = this.metadataSrv.EntityTypes[entityTypeName].Key;
                var resultSerialDataList = DalUtils.LeftJoin(resultSerialDataOriginal.Items, dtos,
                    (ent, dto) => Utils.CompareByKey(JObject.FromObject(ent).ToObject<Dto>(), dto, keyNames),
                    (ent, dto) => this.dataAdapterCud.UpdateEntity(entityTypeName, JObject.FromObject(ent), dto)
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
                        throw new DtosException(code: 204, message: "No Content");
                    }
                    else
                    {
                        throw new DtosException(code: 304, message: "Not Modified");
                    }
                }
            }
        }

        /**
         * Delete single entity
         */
        public ResultSingleSerialData DeleteItem(string entityTypeName, Dto dto)
        {
            var resultSerialDataOriginal = this.dataAdapterRead.FetchOne(entityTypeName, dto, null);
            if (resultSerialDataOriginal.Items.Count() == 0)
            {
                throw new DtosException(code: 404, message: "Not Found");
            }
            else
            {
                this.dataAdapterCud.DeleteEntity(entityTypeName, dto);
                return resultSerialDataOriginal.ToSingle();
            }
        }

        /**
         * Delete multiple entities
         */
        public ResultSerialData DeleteItems(string entityTypeName, IEnumerable<Dto> dtos)
        {
            var resultSerialDataOriginal = this.dataAdapterRead.FetchMany(entityTypeName, dtos, null);
            if (resultSerialDataOriginal.Items.Count() == 0)
            {
                throw new DtosException(code: 404, message: "Not Found");
            }
            else
            {
                foreach (var dto in dtos)
                {
                    this.dataAdapterCud.DeleteEntity(entityTypeName, dto);
                }
                return resultSerialDataOriginal;
            }
        }

        static class Utils
        {

            public static bool CompareByKey(IDictionary<string, JValue> l, IDictionary<string, JValue> r, string[] keyNames)
            {
                var found = true;
                foreach (var keyName in keyNames)
                {
                    found = found && (l[keyName] == r[keyName]);
                }
                return found;
            }

            public static bool KeyPresent(string entityTypeName, Dto dto, MetadataSrv.Metadata metadataSrv)
            {
                var keyNames = metadataSrv.EntityTypes[entityTypeName].Key;
                foreach (var keyName in keyNames)
                {
                    if (!dto.ContainsKey(keyName))
                    {
                        return false;
                    }
                }
                return true;
            }

            public static bool KeysPresent(string entityTypeName, IEnumerable<Dto> dtos, MetadataSrv.Metadata metadataSrv)
            {
                foreach (var dto in dtos)
                {
                    if (!KeyPresent(entityTypeName, dto, metadataSrv))
                    {
                        return false;
                    }
                }
                return true;
            }

        }

    }

}