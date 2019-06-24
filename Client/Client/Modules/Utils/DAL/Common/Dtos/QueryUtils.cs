using System.Collections.Generic;
using System.Linq;

namespace Client.Modules.Utils.DAL.Common
{
    public static class QueryUtils
    {
        public static string RenderQueryString(QueryObject queryObject)
        {
            if (queryObject == null)
            {
                return string.Empty;
            }

            var result = new List<string>();

            if (queryObject.Keys != null && queryObject.Keys.Length > 0)
            {
                result.Add("keys=" + GetStringFromKey(queryObject.Keys));
            }

            if (queryObject.Select != null && queryObject.Select.Length > 0)
            {
                result.Add("select=" + string.Join(",", queryObject.Select));
            }

            if (queryObject.Filter != null)
            {
                result.Add("filter=" + queryObject.Filter);
            }

            if (queryObject.FilterExpand != null && queryObject.FilterExpand.Length > 0)
            {
                result.Add("filterExpand=" + string.Join(",", queryObject.FilterExpand.Select((it) => it.Expand + ":" + (string.IsNullOrEmpty(it.Filter) ? "*" : it.Filter))));
            }

            if (queryObject.OrderBy != null && queryObject.OrderBy.Length > 0)
            {
                result.Add("orderBy=" + string.Join(",", queryObject.OrderBy));
            }

            if (queryObject.Expand != null && queryObject.Expand.Length > 0)
            {
                result.Add("expand=" + string.Join(",", queryObject.Expand));
            }

            if (queryObject.Count != null)
            {
                result.Add("count=" + ((bool)queryObject.Count ? "true" : "false"));
            }

            if (queryObject.Skip != null)
            {
                result.Add("skip=" + queryObject.Skip.ToString());
            }

            if (queryObject.Top != null)
            {
                result.Add("top=" + queryObject.Top.ToString());
            }

            return string.Join("&", result);
        }

        /**
         * keys=[ { key1: 1, key2: 4}, { key1: 2, key2: 5 }, { key1: 2, key2: 6 }, { key1: 4, key2: 7 } ]
         * ... will become:
         * keys=key1:1,2,3,4;key2:4,5,6,7
         */
        private static string GetStringFromKey(Dto[] keys)
        {
            var keySet = new Dictionary<string, List<object>>();
            foreach (var dto in keys)
            {
                foreach (var item in dto)
                {
                    if (!keySet.ContainsKey(item.Key))
                    {
                        keySet.Add(item.Key, new List<object>());
                    }
                    keySet[item.Key].Add(item.Value);
                }
            }
            var result = new List<string>();
            foreach (var item in keySet)
            {
                result.Add(string.Format("{0}:{1}", item.Key, string.Join(",", item.Value)));
            }
            return string.Join(";", result);
        }
    }

}