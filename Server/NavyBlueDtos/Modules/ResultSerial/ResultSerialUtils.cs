using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace NavyBlueDtos
{
    public class ResultSerialUtils
    {
        private readonly DataViewDto dataViewDto;
        private readonly string apiRouteRoot;
        private readonly MetadataSrv.Metadata metadataSrv;

        public ResultSerialUtils(DataViewDto dataViewDto, string apiRouteRoot, MetadataSrv.Metadata metadataSrv)
        {
            this.dataViewDto = dataViewDto;
            this.apiRouteRoot = apiRouteRoot;
            this.metadataSrv = metadataSrv;
        }

        public ResultSerialResponse FetchResponseData(string entityTypeName, QueryObject queryObject)
        {
            var count = this.dataViewDto.Count(entityTypeName, queryObject);
            const int maxTop = 40;
            var skip = queryObject.Skip != null ? (int)queryObject.Skip : 0;
            var topNext = Math.Max(0, queryObject.Top != null ? (int)queryObject.Top : count);
            var top = Math.Min(topNext, maxTop);
            var skipNext = skip + top;
            var nextLinkQueryString = Utils.GetNextLinkQueryString(queryObject, skipNext, topNext);
            var entitySetName = this.metadataSrv.EntityTypes[entityTypeName].EntitySetName;
            var nextLink = skipNext < Math.Min(topNext, count) ? string.Format("api/datasource/{0}/{1}?{2}", this.apiRouteRoot, entitySetName, nextLinkQueryString) : null;
            var queryObjectLocal = Utils.GetQueryObject(entityTypeName, queryObject, skip, top, this.metadataSrv);
            var resultSerialData = this.dataViewDto.GetItems(entityTypeName, queryObjectLocal);
            if (resultSerialData.TotalCount == 0)
            {
                resultSerialData.TotalCount = count;
            }
            return new ResultSerialResponse() { NextLink = nextLink, Data = resultSerialData };
        }

        static class Utils
        {

            public static string GetNextLinkQueryString(QueryObject queryObject, int skip, int top)
            {
                var queryObjectLocal = JObject.FromObject(queryObject).ToObject<QueryObject>();
                queryObjectLocal.Skip = skip;
                queryObjectLocal.Top = top;
                var queryString = QueryUtils.RenderQueryString(queryObjectLocal);
                return queryString;
            }

            private static string[] GetOrderBy(string entityTypeName, MetadataSrv.Metadata metadataSrv)
            {
                var keyNames = metadataSrv.EntityTypes[entityTypeName].Key;
                return keyNames.Select((name) => string.Format("{0} DESC", name)).ToArray();
            }

            public static QueryObject GetQueryObject(string entityTypeName, QueryObject queryObject, int skip, int top, MetadataSrv.Metadata metadataSrv)
            {
                var queryObjectLocal = JObject.FromObject(queryObject).ToObject<QueryObject>();
                if (queryObjectLocal.OrderBy == null || queryObjectLocal.OrderBy.Length == 0)
                {
                    queryObjectLocal.OrderBy = GetOrderBy(entityTypeName, metadataSrv);
                }
                queryObjectLocal.Count = false;
                queryObjectLocal.Skip = skip;
                queryObjectLocal.Top = top;
                return queryObjectLocal;
            }

        }

    }

}