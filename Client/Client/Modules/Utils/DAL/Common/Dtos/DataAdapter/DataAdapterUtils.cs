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

    }

}