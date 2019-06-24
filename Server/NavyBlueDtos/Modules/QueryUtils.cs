using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NavyBlueDtos
{
    public static class QueryUtils
    {
        public static QueryObject RenderQueryObject(QueryParams queryParams)
        {
            var result = new QueryObject();

            if (queryParams.Keys != null)
            {
                result.Keys = GetKeyFromString(queryParams.Keys);
            }

            if (queryParams.Select != null)
            {
                result.Select = queryParams.Select.Split(new char[] { ',' }).Select((it) => it.Trim()).ToArray();
            }

            if (queryParams.Filter != null)
            {
                result.Filter = queryParams.Filter;
            }

            if (queryParams.FilterExpand != null)
            {
                result.FilterExpand = queryParams.FilterExpand.Split(new char[] { ';' }).Select((it) =>
                {
                    var items = it.Split(new char[] { ':' }).Select((it1) => it1.Trim()).ToArray();
                    return new FilterExpand()
                    {
                        Expand = items[0],
                        Filter = items[1] == "*" ? "1" : items[1]
                    };
                }).ToArray();
            }

            if (queryParams.OrderBy != null)
            {
                result.OrderBy = queryParams.OrderBy.Split(new char[] { ',' }).Select((it) => it.Trim()).ToArray();
            }

            if (queryParams.Expand != null)
            {
                result.Expand = queryParams.Expand.Split(new char[] { ',' }).Select((it) => it.Trim()).ToArray();
            }

            if (queryParams.Count != null)
            {
                result.Count = Convert.ToBoolean(queryParams.Count);
            }

            if (queryParams.Skip != null && queryParams.Skip.IsNumeric())
            {
                result.Skip = Convert.ToInt32(queryParams.Skip);
            }

            if (queryParams.Top != null && queryParams.Top.IsNumeric())
            {
                result.Top = Convert.ToInt32(queryParams.Top);
            }

            return result;
        }

        public static string RenderQueryString(QueryObject queryObject)
        {
            var result = new List<string>();

            if (queryObject.Keys != null && queryObject.Keys.Count() > 0)
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

        private static bool IsNumeric(this string input)
        {
            return Regex.IsMatch(input, @"^\d+$");
        }

        /**
         * keys=key1:1,2,3,4;key2:4,5,6,7
         * ... will become:
         * keys=[ { key1: 1, key2: 4}, { key1: 2, key2: 5 }, { key1: 2, key2: 6 }, { key1: 4, key2: 7 } ]
         */
        private static IEnumerable<Dto> GetKeyFromString(string keys)
        {
            var keyValueSet = new Dictionary<string, int[]>();
            var count = 0;
            foreach (var item in keys.Split(new char[] { ';' }))
            {
                var keyValue = item.Split(new char[] { ':' });
                keyValueSet.Add(keyValue[0], keyValue[1].Split(new char[] { ',' }).Select((it) => Convert.ToInt32(it)).ToArray());
                if (count == 0)
                {
                    count = keyValueSet[keyValue[0]].Length;
                }
                else
                {
                    if (count != keyValueSet[keyValue[0]].Length)
                    {
                        throw new ArgumentException("parametru 'keys' incorect");
                    }
                }
            }

            var result = new List<Dto>();
            for (var i = 0; i < count; i++)
            {
                var resultItem = new Dto();
                foreach (var keyValue in keyValueSet)
                {
                    resultItem.Add(keyValue.Key, new JValue(keyValue.Value[i]));
                }
                result.Add(resultItem);
            }
            return result;
        }

        /**
         * keys=[ { key1: 1, key2: 4}, { key1: 2, key2: 5 }, { key1: 2, key2: 6 }, { key1: 4, key2: 7 } ]
         * ... will become:
         * keys=key1:1,2,3,4;key2:4,5,6,7
         */
        private static string GetStringFromKey(IEnumerable<Dto> keys)
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