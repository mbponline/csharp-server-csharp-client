using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Server.Models.Utils.DAL.Common
{
    public static partial class ResultSerialUtils
    {
        public static ResultSerialResponse FetchResponseData(string entityTypeName, QueryObject queryObject, string apiRouteRoot, DataServiceDto dataService)
        {
            var dataView = dataService.DataViewDto;
            var count = dataView.Count(entityTypeName, queryObject);
            const int maxTop = 40;
            var skip = queryObject.Skip != null ? (int)queryObject.Skip : 0;
            var topNext = Math.Max(0, queryObject.Top != null ? (int)queryObject.Top : count);
            var top = Math.Min(topNext, maxTop);
            var skipNext = skip + top;
            var nextLinkQueryString = GetNextLinkQueryString(queryObject, skipNext, topNext);
            var entitySetName = dataService.Metadata.EntityTypes[entityTypeName].EntitySetName;
            var nextLink = skipNext < Math.Min(topNext, count) ? string.Format("api/datasource/{0}/{1}?{2}", apiRouteRoot, entitySetName, nextLinkQueryString) : null;
            var queryObjectLocal = GetQueryObject(entityTypeName, queryObject, skip, top, dataService.Metadata);
            var resultSerial = dataView.GetItems(entityTypeName, queryObjectLocal);
            if (resultSerial.TotalCount == 0)
            {
                resultSerial.TotalCount = count;
            }
            return new ResultSerialResponse() { NextLink = nextLink, Data = resultSerial };
        }

        private static string GetNextLinkQueryString(QueryObject queryObject, int skip, int top)
        {
            var queryObjectLocal = JObject.FromObject(queryObject).ToObject<QueryObject>();
            queryObjectLocal.Skip = skip;
            queryObjectLocal.Top = top;
            var queryString = QueryUtils.RenderQueryString(queryObjectLocal);
            return queryString;
        }

        private static string[] GetOrderBy(string entityTypeName, Metadata metadata)
        {
            var keyNames = metadata.EntityTypes[entityTypeName].Key;
            return keyNames.Select((name) => string.Format("{0} DESC", name)).ToArray();
        }

        private static QueryObject GetQueryObject(string entityTypeName, QueryObject queryObject, int skip, int top, Metadata metadata)
        {
            var queryObjectLocal = JObject.FromObject(queryObject).ToObject<QueryObject>();
            queryObjectLocal.OrderBy = GetOrderBy(entityTypeName, metadata);
            queryObjectLocal.Count = false;
            queryObjectLocal.Skip = skip;
            queryObjectLocal.Top = top;
            return queryObjectLocal;
        }

    }

}