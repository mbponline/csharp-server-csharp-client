using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NavyBlueDtos
{

    internal class DataAdapterRead
    {
        private readonly DatabaseOperations databaseOperations;
        private readonly Dialect dialect;
        private readonly MetadataSrv.Metadata metadataSrv;

        public DataAdapterRead(DatabaseOperations databaseOperations, Dialect dialect, MetadataSrv.Metadata metadataSrv)
        {
            this.databaseOperations = databaseOperations;
            this.dialect = dialect;
            this.metadataSrv = metadataSrv;
        }

        internal int Count(string entityTypeName, QueryObject queryObject)
        {
            var entityTypeQueries = Utils.BuildEntityTypeQueries(entityTypeName, queryObject, this.dialect, this.metadataSrv);
            var countRows = this.databaseOperations.CountQuery(entityTypeQueries.QueryCount);
            return countRows;
        }

        internal ResultSerialData FetchOne(string entityTypeName, Dto dto, string[] expand)
        {
            var keyNames = this.metadataSrv.EntityTypes[entityTypeName].Key;
            var queryObject = new QueryObject()
            {
                Filter = Utils.GetFilterFromKey(keyNames, dto),
                Expand = expand,
                Count = false,
                Top = 1
            };
            return this.Fetch(entityTypeName, queryObject);
        }

        internal ResultSerialData FetchMany(string entityTypeName, IEnumerable<Dto> dtos, string[] expand)
        {
            var keyNames = this.metadataSrv.EntityTypes[entityTypeName].Key;
            var queryObject = new QueryObject()
            {
                Filter = Utils.GetFilterFromKeyMultiple(keyNames, dtos),
                Expand = expand,
                Count = false
            };
            return this.Fetch(entityTypeName, queryObject);
        }

        internal ResultSerialData Fetch(string entityTypeName, QueryObject queryObject)
        {
            var queries = Utils.GetQueryResult(entityTypeName, queryObject, this.dialect, this.metadataSrv);
            var resultSerialData = new ResultSerialData();
            foreach (var et in queries)
            {
                if (!string.IsNullOrEmpty(et.EntityTypeName))
                {
                    var result = this.databaseOperations.ExecuteQuery(et.QueryText);
                    if (et.EntityTypeName == entityTypeName)
                    {
                        resultSerialData.Items = result; //.OfType<Dto>().ToList();
                        resultSerialData.EntityTypeName = entityTypeName;
                    }
                    else
                    {
                        if (resultSerialData.RelatedItems == null)
                        {
                            resultSerialData.RelatedItems = new Dictionary<string, IEnumerable<Dto>>();
                        }
                        resultSerialData.RelatedItems.Add(et.EntityTypeName, result);
                    }
                }
                else
                {
                    var result = this.databaseOperations.CountQuery(et.QueryText);
                    resultSerialData.TotalCount = result;
                }
            }
            return resultSerialData;
        }

        static class Utils
        {

            internal static List<QueryResult> GetQueryResult(string entityTypeName, QueryObject queryObject, Dialect dialect, MetadataSrv.Metadata metadataSrv)
            {
                var result = new List<QueryResult>();

                var entityTypeQueries = BuildEntityTypeQueries(entityTypeName, queryObject, dialect, metadataSrv);

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

                    foreach (var branch in DataUtils.NavigationBranch(splitExpand))
                    {
                        var navs = DataUtils.BranchToNavigation(entityTypeName, branch, metadataSrv);
                        var entityTypeNameLocal = navs[navs.Count - 1].EntityTypeName;
                        var foundEntityType = result.FirstOrDefault((it) => it.EntityTypeName == entityTypeNameLocal);
                        if (foundEntityType == null)
                        {
                            result.Add(new QueryResult()
                            {
                                EntityTypeName = entityTypeNameLocal,
                                QueryText = NavigationBranchToQueryText(entityTypeName, queryRoot, navs, dialect, metadataSrv)
                            });
                        }
                        else
                        {
                            foundEntityType.QueryText += " UNION " + NavigationBranchToQueryText(entityTypeName, queryRoot, navs, dialect, metadataSrv);
                        }
                    }
                }

                return result;
            }

            internal static EntityTypeQueries BuildEntityTypeQueries(string entityTypeName, QueryObject queryObject, Dialect dialect, MetadataSrv.Metadata metadataSrv)
            {
                var select = "it.*";
                var filter = string.Empty;
                var filterExpand = new List<string>();
                var orderBy = string.Empty;
                var skip = string.Empty;
                var top = string.Empty;

                var entityType = metadataSrv.EntityTypes[entityTypeName];

                var fields = entityType.Properties.ToDictionary((it) => it.Key, (it) => it.Value.FieldName);

                var selectFields = fields;
                var hasCustomSelect = queryObject.Select != null && queryObject.Select.Length > 0;
                if (hasCustomSelect)
                {
                    var keyNames = entityType.Key;
                    selectFields = selectFields.Where((it) => keyNames.Contains(it.Key) || queryObject.Select.Contains(it.Key)).ToDictionary((it) => it.Key, (it) => it.Value);
                }
                select = GetSelect(selectFields, hasCustomSelect, dialect);

                var tableName = !string.IsNullOrEmpty(queryObject.CustomQueryTable) ? string.Format("({0})", queryObject.CustomQueryTable) : metadataSrv.EntityTypes[entityTypeName].TableName;
                var entityTypeQuery = new StringBuilder(string.Format("SELECT {0} FROM {1} AS it", "{0}", tableName));

                if (queryObject.FilterExpand != null && queryObject.FilterExpand.Length > 0)
                {
                    var splitExpand = DataUtils.SplitExpand(queryObject.FilterExpand.ToArray(), (el) => el.Expand, (el, it, lastInPath) =>
                    {
                        // 'process' se executa doar la cea din urma proprietate de navigare
                        if (lastInPath)
                        {
                            it.Filter = el.Filter;
                        }
                    });

                    //DataUtils.ForEachNavigationFilter(entityTypeName, splitExpand, (parentEntityTypeName, nav, filterExp) =>
                    //{
                    //    var tableNameLocal = metadataSrv.EntityTypes[entityTypeName].TableName;
                    //    var join = GetJoinCondition(parentEntityTypeName, entityTypeName == parentEntityTypeName, nav, false, metadataSrv);
                    //    tableNameLocal = metadataSrv.EntityTypes[nav.EntityTypeName].TableName;

                    //    entityTypeQuery.Append(string.Format(" INNER JOIN {0} ON {1}", tableNameLocal, join));

                    //    if (!string.IsNullOrEmpty(filterExp))
                    //    {
                    //        var childElementFields = metadataSrv.EntityTypes[nav.EntityTypeName].Properties.ToDictionary((it) => it.Key, (it) => it.Value.FieldName);
                    //        filterExpand.Add(string.Format("( {0} )", ReplaceFieldNames(tableNameLocal, filterExp, childElementFields, dialect)));
                    //    }
                    //}, metadataSrv);

                    foreach (var navigationResult in DataUtils.NavigationFilter(entityTypeName, splitExpand, metadataSrv))
                    {
                        var parentEntityTypeName = navigationResult.EntityTypeName;
                        var nav = navigationResult.NavigationProperty;
                        var filterExp = navigationResult.Filter;

                        var tableNameLocal = metadataSrv.EntityTypes[entityTypeName].TableName;
                        var join = GetJoinCondition(parentEntityTypeName, entityTypeName == parentEntityTypeName, nav, false, metadataSrv);
                        tableNameLocal = metadataSrv.EntityTypes[nav.EntityTypeName].TableName;

                        entityTypeQuery.Append(string.Format(" INNER JOIN {0} ON {1}", tableNameLocal, join));

                        if (!string.IsNullOrEmpty(filterExp))
                        {
                            var childElementFields = metadataSrv.EntityTypes[nav.EntityTypeName].Properties.ToDictionary((it) => it.Key, (it) => it.Value.FieldName);
                            filterExpand.Add(string.Format("( {0} )", ReplaceFieldNames(tableNameLocal, filterExp, childElementFields, dialect)));
                        }
                    }
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
                foreach (var propertyName in resultFields)
                {
                    var rgx = new Regex("\\b" + propertyName.Key + "\\b");
                    result = rgx.Replace(result, string.Format(fieldTemplate, tableName, propertyName.Value));
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

            internal static string NavigationBranchToQueryText(string entityTypeName, string entityTypeQuery, List<MetadataSrv.NavigationProperty> navBranch, Dialect dialect, MetadataSrv.Metadata metadataSrv)
            {
                // A INNER JOIN B ON A.IdDepartament = B.IdDepartament AND A.IdPersoana = B.IdPersoana INNER JOIN C ON ...
                var entityTypeNameLocal = entityTypeName;
                var navLast = navBranch[navBranch.Count - 1];
                var distinct = navLast.Multiplicity == "single" ? "DISTINCT" : string.Empty;
                var fields = metadataSrv.EntityTypes[navLast.EntityTypeName].Properties.ToDictionary((it) => it.Key, (it) => it.Value.FieldName);
                var result = new StringBuilder(string.Format("SELECT {0} {1} FROM ", distinct, GetSelect(fields, false, dialect)));
                var tableName = metadataSrv.EntityTypes[entityTypeName].TableName;
                result.Append(!string.IsNullOrEmpty(entityTypeQuery) ? string.Format("({0}) AS {1}", entityTypeQuery, tableName) : tableName);
                for (var i = 0; i < navBranch.Count - 1; i++)
                {
                    var nav = navBranch[i];
                    result.Append(string.Format(" INNER JOIN {0} ON {1}", metadataSrv.EntityTypes[nav.EntityTypeName].TableName, GetJoinCondition(entityTypeNameLocal, false, nav, false, metadataSrv)));
                    entityTypeNameLocal = nav.EntityTypeName;
                }
                result.Append(string.Format(" INNER JOIN {0} AS it ON {1}", metadataSrv.EntityTypes[navLast.EntityTypeName].TableName, GetJoinCondition(entityTypeNameLocal, false, navLast, true, metadataSrv)));
                return result.ToString();
            }

            internal static string GetJoinCondition(string entityTypeName, bool entityTypeTableAlias, MetadataSrv.NavigationProperty navigationProperty, bool navigationTableAlias, MetadataSrv.Metadata metadataSrv)
            {
                // t1.prop1 = t2.prop1 AND t1.prop2 = t2.prop2
                var result = new List<string>();
                var entityType = metadataSrv.EntityTypes[entityTypeName];
                for (var i = 0; i < navigationProperty.KeyLocal.Length; i++)
                {
                    var entityTypeNav = metadataSrv.EntityTypes[navigationProperty.EntityTypeName];
                    result.Add((entityTypeTableAlias ? "it" : entityType.TableName) + "." + entityType.Properties[navigationProperty.KeyLocal[i]].FieldName + " = " + (navigationTableAlias ? "it" : entityTypeNav.TableName) + "." + entityTypeNav.Properties[navigationProperty.KeyRemote[i]].FieldName);
                }
                return string.Join(" AND ", result);
            }

            internal static string GetFilterFromKey(string[] keyNames, Dto dto)
            {
                var result = new List<string>();
                if (keyNames.Length == 0)
                {
                    throw new Exception("Invalid key");
                }
                foreach (var keyName in keyNames)
                {
                    if (!dto.ContainsKey(keyName))
                    {
                        throw new Exception("Key fiels not present");
                    }
                    result.Add(keyName + "=" + dto[keyName]);
                }
                return string.Join(" AND ", result);
            }

            internal static string GetFilterFromKeyMultiple(string[] keyNames, IEnumerable<Dto> dtos)
            {
                var result = new List<string>();
                if (keyNames.Length == 0)
                {
                    throw new Exception("Invalid key");
                }
                if (keyNames.Length == 1)
                {
                    return keyNames[0] + " IN (" + string.Join(",", dtos.Select((it) => it[keyNames[0]])) + ")";
                }
                foreach (var dto in dtos)
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

}
