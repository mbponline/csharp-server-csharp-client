using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Server.Models.Utils.DAL.Common
{

    internal static class CudUtils
    {
        public static ResultSerialData UpdateEntity(string entityTypeName, JObject entity, Dto dto, Metadata metadata, Dialect dialect, string connectionString, bool returnUpdated = false)
        {
            var qryKeyValues = new List<string>();
            var keyNames = metadata.EntityTypes[entityTypeName].Key;

            var fields = metadata.EntityTypes[entityTypeName].Properties.ToDictionary((it) => it.Key, (it) => it.Value.FieldName);

            foreach (var prop in dto)
            {
                if (keyNames.Contains(prop.Key))
                {
                    qryKeyValues.Add(string.Format("{0} = {1}", fields[prop.Key], prop.Value));
                }
            }
            var qryKey = string.Join(" AND ", qryKeyValues);

            var tableName = metadata.EntityTypes[entityTypeName].TableName;
            var calculatedProperties = metadata.EntityTypes[entityTypeName].CalculatedProperties;

            var qrySetValues = new List<string>();

            var entityLocal = entity.ToObject<Dictionary<string, object>>();

            foreach (var prop in dto)
            {
                if (entityLocal.ContainsKey(prop.Key) && entityLocal[prop.Key] != dto[prop.Key])
                {
                    if (!IsCalculated(calculatedProperties, prop.Key))
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
                DatabaseOperations.CudQuery(qryUpdate, dialect, connectionString);
                return returnUpdated ? ReadUtils.FetchOne(entityTypeName, dto, null, metadata, dialect, connectionString) : null;
            }
            else
            {
                return null;
            }
        }

        public static ResultSerialData InsertEntity(string entityTypeName, Dto dto, Metadata metadata, Dialect dialect, string connectionString)
        {
            var qryFieldsNames = new List<string>();
            var qryFieldsValues = new List<string>();

            var tableName = metadata.EntityTypes[entityTypeName].TableName;
            var calculatedProperties = metadata.EntityTypes[entityTypeName].CalculatedProperties;

            var fields = metadata.EntityTypes[entityTypeName].Properties.ToDictionary((it) => it.Key, (it) => it.Value.FieldName);

            foreach (var prop in dto)
            {
                if (!IsCalculated(calculatedProperties, prop.Key))
                {
                    qryFieldsNames.Add(fields[prop.Key]);
                    qryFieldsValues.Add(string.Format("'{0}'", prop.Value));
                }
            }

            if (qryFieldsValues.Count > 0)
            {
                var qryInsert = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", tableName, string.Join(",", qryFieldsNames), string.Join(",", qryFieldsValues));
                var result = DatabaseOperations.CudQuery(qryInsert, dialect, connectionString);
                var identityPropertyName = GetIdentityPropertyName(calculatedProperties, metadata.EntityTypes[entityTypeName].Key);
                if (!string.IsNullOrEmpty(identityPropertyName))
                {
                    var dtonew = new Dto() { { identityPropertyName, (int)result } };
                    return ReadUtils.FetchOne(entityTypeName, dtonew, null, metadata, dialect, connectionString);
                }
                else
                {
                    return ReadUtils.FetchOne(entityTypeName, dto, null, metadata, dialect, connectionString);
                }
            }
            else
            {
                return null;
            }
        }

        public static void DeleteEntity(string entityTypeName, Dto dto, Metadata metadata, Dialect dialect, string connectionString)
        {
            var qryKeyValues = new List<string>();
            var keyNames = metadata.EntityTypes[entityTypeName].Key;
            for (int i = 0; i < keyNames.Length; i++)
            {
                qryKeyValues.Add(string.Format("{0} = {1}", keyNames[i], dto[keyNames[i]]));
            }
            var qryKey = string.Join(" AND ", qryKeyValues);
            var tableName = metadata.EntityTypes[entityTypeName].TableName;
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

            DatabaseOperations.CudQuery(qryDelete, dialect, connectionString);
        }

        public static bool CompareByKey(IDictionary<string, object> l, IDictionary<string, object> r, string[] keyNames)
        {
            var found = true;
            foreach (var keyName in keyNames)
            {
                found = found && (l[keyName] == r[keyName]);
            }
            return found;
        }

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



        public static bool KeyPresent(string entityTypeName, Dto dto, Metadata metadata)
        {
            var keyNames = metadata.EntityTypes[entityTypeName].Key;
            foreach (var keyName in keyNames)
            {
                if (!dto.ContainsKey(keyName))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool KeysPresent(string entityTypeName, Dto[] dtos, Metadata metadata)
        {
            foreach (var dto in dtos)
            {
                if (!KeyPresent(entityTypeName, dto, metadata))
                {
                    return false;
                }
            }
            return true;
        }

    }

}