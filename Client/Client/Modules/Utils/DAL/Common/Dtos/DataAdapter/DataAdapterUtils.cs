using System;
using System.Collections.Generic;
using System.Linq;

namespace Client.Modules.Utils.DAL.Common
{

    internal static class DataAdapterUtils
    {
        public static Dto GetKeyFromData(string[] keyNames, Dto dto)
        {
            var result = new Dto();
            foreach (var keyName in keyNames)
            {
                if (dto.ContainsKey(keyName))
                {
                    result[keyName] = dto[keyName];
                }
                else
                {
                    throw new Exception("Invalid dto");
                }
            }
            return result;
        }

        public static IEnumerable<Dto> GetKeyFromMultipleData(string[] keyNames, Dto[] dtos)
        {
            var result = dtos.Select(dto => GetKeyFromData(keyNames, dto));
            return result;
        }

        public static string CreateParamsQueryString(Dictionary<string, object> paramList)
        {
            var result = new List<string>();
            foreach (var it in paramList)
            {
                if (it.Value.GetType() == typeof(bool))
                {
                    result.Add(string.Format("{0}={1}", it.Key, it.Value.ToString()));
                }
                else if (it.Value.GetType() == typeof(DateTime))
                {
                    result.Add(string.Format("{0}={1}", it.Key, it.Value.ToString()));
                }
                else
                {
                    result.Add(string.Format("{0}={1}", it.Key, it.Value.ToString()));
                }
            }
            return string.Join("&", result).Replace("=null", "=");
        }

    }

}