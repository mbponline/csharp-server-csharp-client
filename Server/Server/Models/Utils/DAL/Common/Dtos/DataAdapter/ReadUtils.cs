using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Models.Utils.DAL.Common
{

    internal static class ReadUtils
    {
        internal static int Count(string entityTypeName, QueryObject queryObject, Metadata metadata, Dialect dialect, string connectionString)
        {
            var entityTypeQueries = BuildEntityTypeQueries(entityTypeName, queryObject, metadata, dialect);
            var countRows = DatabaseOperations.CountQuery(entityTypeQueries.QueryCount, dialect, connectionString);
            return countRows;
        }

        internal static ResultSerialData FetchOne(string entityTypeName, Dto dto, string[] expand, Metadata metadata, Dialect dialect, string connectionString)
        {
            var keyNames = metadata.EntityTypes[entityTypeName].Key;
            var queryObject = new QueryObject()
            {
                Filter = GetFilterFromKey(keyNames, dto),
                Expand = expand,
                Count = false,
                Top = 1
            };
            return Fetch(entityTypeName, queryObject, metadata, dialect, connectionString);
        }

        internal static ResultSerialData FetchMany(string entityTypeName, Dto[] dtos, string[] expand, Metadata metadata, Dialect dialect, string connectionString)
        {
            var keyNames = metadata.EntityTypes[entityTypeName].Key;
            var queryObject = new QueryObject()
            {
                Filter = GetFilterFromKeyMultiple(keyNames, dtos),
                Expand = expand,
                Count = false
            };
            return ReadUtils.Fetch(entityTypeName, queryObject, metadata, dialect, connectionString);
        }

        internal static ResultSerialData Fetch(string entityTypeName, QueryObject queryObject, Metadata metadata, Dialect dialect, string connectionString)
        {
            var queries = GetQueryResult(entityTypeName, queryObject, metadata, dialect);
            var resultSerialData = new ResultSerialData();
            foreach (var et in queries)
            {
                if (!string.IsNullOrEmpty(et.EntityTypeName))
                {
                    var result = DatabaseOperations.ExecuteQuery(et.QueryText, dialect, connectionString);
                    if (et.EntityTypeName == entityTypeName)
                    {
                        resultSerialData.Items = result.ToList();
                        resultSerialData.EntityTypeName = entityTypeName;
                    }
                    else
                    {
                        if (resultSerialData.RelatedItems == null)
                        {
                            resultSerialData.RelatedItems = new Dictionary<string, IEnumerable<object>>();
                        }
                        resultSerialData.RelatedItems.Add(et.EntityTypeName, result.OfType<object>().ToList());
                    }
                }
                else
                {
                    var result = DatabaseOperations.CountQuery(et.QueryText, dialect, connectionString);
                    resultSerialData.TotalCount = result;
                }
            }
            return resultSerialData;
        }

        internal static List<QueryResult> GetQueryResult(string entityTypeName, QueryObject queryObject, Metadata metadata, Dialect dialect)
        {
            var result = new List<QueryResult>();

            var entityTypeQueries = BuildEntityTypeQueries(entityTypeName, queryObject, metadata, dialect);

            var queryRoot = entityTypeQueries.QueryRoot;

            result.Add(new QueryResult()
            {
                EntityTypeName = entityTypeName,
                QueryText = entityTypeQueries.QuerySelect
            });

            if (queryObject.Count != null)
            {
                result.Add(new QueryResult()
                {
                    EntityTypeName = null,
                    QueryText = entityTypeQueries.QueryCount
                });
            }

            if (queryObject.Expand != null && queryObject.Expand.Length > 0)
            {
                var splitExpand = DataUtils.SplitExpand(queryObject.Expand, (el) => el);
                DataUtils.ForEachNavigation(splitExpand, (branch) =>
                {
                    var navs = DataUtils.BranchToNavigation(entityTypeName, branch, metadata);
                    var entityTypeNameLocal = navs[navs.Count - 1].EntityTypeName;
                    var foundEntityType = result.FirstOrDefault((it) => it.EntityTypeName == entityTypeNameLocal);
                    if (foundEntityType == null)
                    {
                        result.Add(new QueryResult()
                        {
                            EntityTypeName = entityTypeNameLocal,
                            QueryText = NavigationBranchToQueryText(entityTypeName, queryRoot, navs, metadata, dialect)
                        });
                    }
                    else
                    {
                        foundEntityType.QueryText += " UNION " + NavigationBranchToQueryText(entityTypeName, queryRoot, navs, metadata, dialect);
                    }
                });
            }

            return result;
        }

        internal static EntityTypeQueries BuildEntityTypeQueries(string entityTypeName, QueryObject queryObject, Metadata metadata, Dialect dialect)
        {
            var select = "it.*";
            var filter = string.Empty;
            var filterExpand = new List<string>();
            var orderBy = string.Empty;
            var skip = string.Empty;
            var top = string.Empty;

            var entityType = metadata.EntityTypes[entityTypeName];

            var fields = entityType.Properties.ToDictionary((it) => it.Key, (it) => it.Value.FieldName);

            var selectFields = fields;
            var hasCustomSelect = queryObject.Select != null && queryObject.Select.Length > 0;
            if (hasCustomSelect)
            {
                var keyNames = entityType.Key;
                selectFields = selectFields.Where((it) => keyNames.Contains(it.Key) || queryObject.Select.Contains(it.Key)).ToDictionary((it) => it.Key, (it) => it.Value);
            }
            select = GetSelect(selectFields, hasCustomSelect, dialect);

            var tableName = !string.IsNullOrEmpty(queryObject.CustomQueryTable) ? string.Format("({0})", queryObject.CustomQueryTable) : metadata.EntityTypes[entityTypeName].TableName;
            var entityTypeQuery = new StringBuilder(string.Format("SELECT {0} FROM {1} AS it", "{0}", tableName));

            if (queryObject.FilterExpand != null && queryObject.FilterExpand.Count > 0)
            {
                var splitExpand = DataUtils.SplitExpand(queryObject.FilterExpand.ToArray(), (el) => el.Expand, (el, it, lastInPath) =>
                {
                    // 'process' se executa doar la cea din urma proprietate de navigare
                    if (lastInPath)
                    {
                        it.Filter = el.Filter;
                    }
                });

                DataUtils.ForEachNavigationFilter(entityTypeName, splitExpand, (parentEntityTypeName, nav, filterExp) =>
                {
                    var tableNameLocal = metadata.EntityTypes[entityTypeName].TableName;
                    var join = GetJoinCondition(parentEntityTypeName, entityTypeName == parentEntityTypeName, nav, false, metadata);
                    tableNameLocal = metadata.EntityTypes[nav.EntityTypeName].TableName;

                    entityTypeQuery.Append(string.Format(" INNER JOIN {0} ON {1}", tableNameLocal, join));

                    if (!string.IsNullOrEmpty(filterExp))
                    {
                        var childElementFields = metadata.EntityTypes[nav.EntityTypeName].Properties.ToDictionary((it) => it.Key, (it) => it.Value.FieldName);
                        filterExpand.Add(string.Format("( {0} )", ReplaceFieldNames(tableNameLocal, filterExp, childElementFields, dialect)));
                    }
                }, metadata);
            }

            var arrFilter = new List<string>();
            if (queryObject.Filter != null)
            {
                arrFilter.Add(string.Format("( {0} )", ReplaceFieldNames("it", queryObject.Filter, fields, dialect)));
            }
            if (filterExpand.Count > 0)
            {
                arrFilter.AddRange(filterExpand);
            }
            if (arrFilter.Count > 0)
            {
                filter = " WHERE " + string.Join(" AND ", arrFilter);
            }

            if (queryObject.OrderBy != null && queryObject.OrderBy.Length > 0)
            {
                orderBy = " ORDER BY " + string.Join(", ", queryObject.OrderBy);
                orderBy = ReplaceFieldNames("it", orderBy, fields, dialect);
            }

            switch (dialect)
            {
                case Dialect.SQL2012:
                case Dialect.SQL2014:
                    if (queryObject.Top != null)
                    {
                        top = string.Format(" FETCH NEXT {0} ROWS ONLY", queryObject.Top);
                    }

                    if (queryObject.Skip != null)
                    {
                        skip = string.Format(" OFFSET {0} ROWS", queryObject.Skip);
                    }
                    break;
                case Dialect.MYSQL:
                    if (queryObject.Top != null)
                    {
                        top = " LIMIT " + queryObject.Top;
                    }

                    if (queryObject.Skip != null)
                    {
                        skip = " OFFSET " + queryObject.Skip;
                    }
                    break;
                default:
                    break;
            }
            var queryCount = "COUNT(*) AS count";
            queryCount = string.Format(entityTypeQuery.ToString() + filter, queryCount);
            entityTypeQuery.Append(filter + orderBy + top + skip);

            var querySelect = string.Format(entityTypeQuery.ToString(), select);
            var queryRoot = string.Format(entityTypeQuery.ToString(), "it.*");

            return new EntityTypeQueries()
            {
                QuerySelect = querySelect,
                QueryCount = queryCount,
                QueryRoot = queryRoot
            };
        }

        private static string ReplaceFieldNames(string tableName, string source, Dictionary<string, string> fields, Dialect dialect)
        {
            var fieldTemplate = string.Empty;
            switch (dialect)
            {
                case Dialect.SQL2012:
                case Dialect.SQL2014:
                    fieldTemplate = "{0}.[{1}]";
                    break;
                case Dialect.MYSQL:
                    fieldTemplate = "`{0}`.`{1}`";
                    break;
            }
            var result = source;
            var resultFields = fields.Where((it) => source.Contains(it.Key)).OrderByDescending((it) => it.Key.Length);
            foreach (var item in resultFields)
            {
                result = result.Replace(item.Key, string.Format(fieldTemplate, tableName, item.Value));
            }
            return result;
        }

        private static string GetSelect(Dictionary<string, string> selectFields, bool forceFieldNames, Dialect dialect)
        {
            var selectTemplate = string.Empty;
            switch (dialect)
            {
                case Dialect.SQL2012:
                case Dialect.SQL2014:
                    selectTemplate = "it.[{0}] AS {1}";
                    break;
                case Dialect.MYSQL:
                    selectTemplate = "it.`{0}` AS {1}";
                    break;
            }
            var select = "it.*";
            var useFieldNames = true;
            if (!forceFieldNames)
            {
                useFieldNames = selectFields.Any((it) => it.Key != it.Value);
            }
            if (useFieldNames)
            {
                var selectList = selectFields.Select((it) => it.Value == it.Key ? "it." + it.Key : string.Format(selectTemplate, it.Value, it.Key));
                select = string.Join(", ", selectList);
            }
            return select;
        }

        internal static string NavigationBranchToQueryText(string entityTypeName, string entityTypeQuery, List<NavigationProperty> navBranch, Metadata metadata, Dialect dialect)
        {
            // A INNER JOIN B ON A.IdDepartament = B.IdDepartament AND A.IdPersoana = B.IdPersoana INNER JOIN C ON ...
            var entityTypeNameLocal = entityTypeName;
            var navLast = navBranch[navBranch.Count - 1];
            var distinct = navLast.Multiplicity == "single" ? "DISTINCT" : string.Empty;
            var fields = metadata.EntityTypes[navLast.EntityTypeName].Properties.ToDictionary((it) => it.Key, (it) => it.Value.FieldName);
            var result = new StringBuilder(string.Format("SELECT {0} {1} FROM ", distinct, GetSelect(fields, false, dialect)));
            var tableName = metadata.EntityTypes[entityTypeName].TableName;
            result.Append(!string.IsNullOrEmpty(entityTypeQuery) ? string.Format("({0}) AS {1}", entityTypeQuery, tableName) : tableName);
            for (var i = 0; i < navBranch.Count - 1; i++)
            {
                var nav = navBranch[i];
                result.Append(string.Format(" INNER JOIN {0} ON {1}", metadata.EntityTypes[nav.EntityTypeName].TableName, GetJoinCondition(entityTypeNameLocal, false, nav, false, metadata)));
                entityTypeNameLocal = nav.EntityTypeName;
            }
            result.Append(string.Format(" INNER JOIN {0} AS it ON {1}", metadata.EntityTypes[navLast.EntityTypeName].TableName, GetJoinCondition(entityTypeNameLocal, false, navLast, true, metadata)));
            return result.ToString();
        }

        internal static string GetJoinCondition(string entityTypeName, bool entityTypeTableAlias, NavigationProperty navigationProperty, bool navigationTableAlias, Metadata metadata)
        {
            // t1.prop1 = t2.prop1 AND t1.prop2 = t2.prop2
            var result = new List<string>();
            var entityType = metadata.EntityTypes[entityTypeName];
            for (var i = 0; i < navigationProperty.KeyLocal.Length; i++)
            {
                var entityTypeNav = metadata.EntityTypes[navigationProperty.EntityTypeName];
                result.Add((entityTypeTableAlias ? "it" : entityType.TableName) + "." + entityType.Properties[navigationProperty.KeyLocal[i]].FieldName + " = " + (navigationTableAlias ? "it" : entityTypeNav.TableName) + "." + entityTypeNav.Properties[navigationProperty.KeyRemote[i]].FieldName);
            }
            return string.Join(" AND ", result);
        }

        internal static string GetFilterFromKey(string[] keyNames, Dto partialEntity)
        {
            var result = new List<string>();
            if (keyNames.Length == 0)
            {
                throw new Exception("Invalid key");
            }
            foreach (var keyName in keyNames)
            {
                if (!partialEntity.ContainsKey(keyName))
                {
                    throw new Exception("Key field not present");
                }
                result.Add(keyName + "=" + partialEntity[keyName]);
            }
            return string.Join(" AND ", result);
        }

        internal static string GetFilterFromKeyMultiple(string[] keyNames, Dto[] partialEntity)
        {
            var result = new List<string>();
            if (keyNames.Length == 0)
            {
                throw new Exception("Invalid key");
            }
            if (keyNames.Length == 1)
            {
                return keyNames[0] + " IN (" + string.Join(",", partialEntity.Select((it) => it[keyNames[0]])) + ")";
            }
            foreach (var dto in partialEntity)
            {
                var dtoFilter = new List<string>();
                foreach (var keyName in keyNames)
                {
                    dtoFilter.Add(keyName + "=" + dto[keyName]);
                }
                result.Add(string.Join(" AND ", dtoFilter));
            }
            return string.Join(" OR ", result);
        }
    }

}