using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NavyBlueDtos
{
    public static class ApiProviderDtoUtils
    {
        public static string FixEntitySetNameCase(string entitySetName, MetadataSrv.Metadata metadataSrv)
        {
            foreach (var prop in metadataSrv.EntityTypes)
            {
                if (metadataSrv.EntityTypes[prop.Key].EntitySetName.ToLower() == entitySetName.ToLower())
                {
                    return metadataSrv.EntityTypes[prop.Key].EntitySetName;
                }
            }
            throw new Exception("Invalid entitySetName");
        }

        public static string GetEntityTypeName(string entitySetName, MetadataSrv.Metadata metadataSrv)
        {
            //var entityType = (from t in metadataSrv.entityTypes where t.Value.entitySetName == entitySetName select t).FirstOrDefault();
            //return entityType.Key;
            var result = string.Empty;
            foreach (var prop in metadataSrv.EntityTypes)
            {
                if (metadataSrv.EntityTypes[prop.Key].EntitySetName == entitySetName)
                {
                    result = prop.Key;
                }
            }
            return result;
        }

        public static bool ValidKeys(string entityTypeName, IEnumerable<Dto> dtos, MetadataSrv.Metadata metadataSrv)
        {
            if (dtos == null || dtos.Count() == 0)
            {
                return false;
            }
            var keyNames = metadataSrv.EntityTypes[entityTypeName].Key;
            foreach (var dto in dtos)
            {
                if (!ValidDtoKey(entityTypeName, dto, metadataSrv))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ValidDtoKey(string entityTypeName, Dto dto, MetadataSrv.Metadata metadataSrv)
        {
            var keyNames = metadataSrv.EntityTypes[entityTypeName].Key;
            foreach (var name in keyNames)
            {
                if (!(dto.ContainsKey(name) || Regex.IsMatch(dto[name].ToString(), @"^\d+$")))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ValidDto(string entityTypeName, Dto key, Dto dto, MetadataSrv.Metadata metadataSrv)
        {
            var keyNames = metadataSrv.EntityTypes[entityTypeName].Key;
            // if dto has key fields, their values should match the key values from query string
            foreach (var name in keyNames)
            {
                if (dto.ContainsKey(name) && dto[name] != key[name])
                {
                    return false;
                }
            }
            return true;
        }
    }

}