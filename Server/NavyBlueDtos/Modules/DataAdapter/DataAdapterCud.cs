using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace NavyBlueDtos
{

    internal class DataAdapterCud
    {
        private readonly DataAdapterRead dataAdapterRead;
        private readonly DatabaseOperations databaseOperations;
        private readonly Dialect dialect;
        private MetadataSrv.Metadata metadataSrv;

        public DataAdapterCud(DataAdapterRead dataAdapterRead, DatabaseOperations databaseOperations, Dialect dialect, MetadataSrv.Metadata metadataSrv)
        {
            this.dataAdapterRead = dataAdapterRead;
            this.databaseOperations = databaseOperations;
            this.dialect = dialect;
            this.metadataSrv = metadataSrv;
        }

        public ResultSerialData UpdateEntity(string entityTypeName, JObject entity, Dto dto, bool returnUpdated = false)
        {
            var qryKeyValues = new List<string>();
            var keyNames = this.metadataSrv.EntityTypes[entityTypeName].Key;

            var fields = metadataSrv.EntityTypes[entityTypeName].Properties.ToDictionary((it) => it.Key, (it) => it.Value.FieldName);

            foreach (var prop in dto)
            {
                if (keyNames.Contains(prop.Key))
                {
                    qryKeyValues.Add(string.Format("{0} = {1}", fields[prop.Key], prop.Value));
                }
            }
            var qryKey = string.Join(" AND ", qryKeyValues);

            var tableName = metadataSrv.EntityTypes[entityTypeName].TableName;
            var calculatedProperties = metadataSrv.EntityTypes[entityTypeName].CalculatedProperties;

            var qrySetValues = new List<string>();

            var entityLocal = entity.ToObject<Dictionary<string, object>>();

            foreach (var prop in dto)
            {
                if (entityLocal.ContainsKey(prop.Key) && entityLocal[prop.Key] != dto[prop.Key])
                {
                    if (!Utils.IsCalculated(calculatedProperties, prop.Key))
                    {
                        qrySetValues.Add(string.Format("{0} = '{1}'", fields[prop.Key], dto[prop.Key]));
                    }
                }
            }
            var qrySet = string.Join(", ", qrySetValues);

            if (qrySetValues.Count > 0)
            {
                var qryUpdate = string.Empty;
                switch (dialect)
                {
                    case Dialect.SQL2012:
                    case Dialect.SQL2014:
                        qryUpdate = string.Format("UPDATE TOP (1) {0} SET {1} WHERE {2}", tableName, qrySet, qryKey);
                        break;
                    case Dialect.MYSQL:
                        qryUpdate = string.Format("UPDATE {0} SET {1} WHERE {2} LIMIT 1", tableName, qrySet, qryKey);
                        break;
                    default:
                        break;
                }
                this.databaseOperations.CudQuery(qryUpdate);
                return returnUpdated ? this.dataAdapterRead.FetchOne(entityTypeName, dto, null) : null;
            }
            else
            {
                return null;
            }
        }

        public ResultSerialData InsertEntity(string entityTypeName, Dto dto)
        {
            var qryFieldsNames = new List<string>();
            var qryFieldsValues = new List<string>();

            var tableName = metadataSrv.EntityTypes[entityTypeName].TableName;
            var calculatedProperties = metadataSrv.EntityTypes[entityTypeName].CalculatedProperties;

            var fields = metadataSrv.EntityTypes[entityTypeName].Properties.ToDictionary((it) => it.Key, (it) => it.Value.FieldName);

            foreach (var prop in dto)
            {
                if (!Utils.IsCalculated(calculatedProperties, prop.Key))
                {
                    qryFieldsNames.Add(fields[prop.Key]);
                    qryFieldsValues.Add(string.Format("'{0}'", prop.Value));
                }
            }

            if (qryFieldsValues.Count > 0)
            {
                var qryInsert = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", tableName, string.Join(",", qryFieldsNames), string.Join(",", qryFieldsValues));
                var result = this.databaseOperations.CudQuery(qryInsert);
                var identityPropertyName = Utils.GetIdentityPropertyName(calculatedProperties, metadataSrv.EntityTypes[entityTypeName].Key);
                if (!string.IsNullOrEmpty(identityPropertyName))
                {
                    var dtonew = new Dto() { { identityPropertyName, new JValue(result) } };
                    return this.dataAdapterRead.FetchOne(entityTypeName, dtonew, null);
                }
                else
                {
                    return this.dataAdapterRead.FetchOne(entityTypeName, dto, null);
                }
            }
            else
            {
                return null;
            }
        }

        public void DeleteEntity(string entityTypeName, Dto dto)
        {
            var qryKeyValues = new List<string>();
            var keyNames = metadataSrv.EntityTypes[entityTypeName].Key;
            for (int i = 0; i < keyNames.Length; i++)
            {
                qryKeyValues.Add(string.Format("{0} = {1}", keyNames[i], dto[keyNames[i]]));
            }
            var qryKey = string.Join(" AND ", qryKeyValues);
            var tableName = metadataSrv.EntityTypes[entityTypeName].TableName;
            var qryDelete = string.Empty;
            switch (dialect)
            {
                case Dialect.SQL2012:
                case Dialect.SQL2014:
                    qryDelete = string.Format("DELETE TOP (1) FROM {0} WHERE {1}", tableName, qryKey);
                    break;
                case Dialect.MYSQL:
                    qryDelete = string.Format("DELETE FROM {0} WHERE {1} LIMIT 1", tableName, qryKey);
                    break;
                default:
                    break;
            }

            this.databaseOperations.CudQuery(qryDelete);
        }

        static class Utils
        {

            public static bool IsCalculated(string[] calculatedProperties, string property)
            {
                var isCalculated = calculatedProperties != null && calculatedProperties.Length > 0 && calculatedProperties.Contains(property);
                return isCalculated;
            }

            public static string GetIdentityPropertyName(string[] calculatedProperties, string[] keyNames)
            {
                var keyName = string.Empty;
                if (keyNames.Length == 1 && calculatedProperties != null && calculatedProperties.Length > 0 && calculatedProperties.Contains(keyNames[0]))
                {
                    keyName = keyNames[0];
                }
                return keyName;
            }

        }

    }

}