using System;
using System.Text.RegularExpressions;

namespace Server.Models.Utils.DAL.Common
{
    public static class ApiProviderUtils
    {
        public static string FixEntitySetNameCase(string entitySetName, Metadata metadata)
        {
            foreach (var prop in metadata.EntityTypes)
            {
                if (metadata.EntityTypes[prop.Key].EntitySetName.ToLower() == entitySetName.ToLower())
                {
                    return metadata.EntityTypes[prop.Key].EntitySetName;
                }
            }
            throw new Exception("Invalid entitySetName");
        }

        public static string GetEntityTypeName(string entitySetName, Metadata metadata)
        {
            //var entityType = (from t in metadata.entityTypes where t.Value.entitySetName == entitySetName select t).FirstOrDefault();
            //return entityType.Key;
            var result = string.Empty;
            foreach (var prop in metadata.EntityTypes)
            {
                if (metadata.EntityTypes[prop.Key].EntitySetName == entitySetName)
                {
                    result = prop.Key;
                }
            }
            return result;
        }

        public static bool ValidKeys(string entityTypeName, Dto[] dtos, Metadata metadata)
        {
            if (dtos == null || dtos.Length == 0)
            {
                return false;
            }
            var keyNames = metadata.EntityTypes[entityTypeName].Key;
            foreach (var dto in dtos)
            {
                if (!ValidDtoKey(entityTypeName, dto, metadata))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ValidDtoKey(string entityTypeName, Dto dto, Metadata metadata)
        {
            var keyNames = metadata.EntityTypes[entityTypeName].Key;
            foreach (var name in keyNames)
            {
                if (!(dto.ContainsKey(name) || Regex.IsMatch(dto[name].ToString(), @"^\d+$")))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ValidDto(string entityTypeName, Dto key, Dto dto, Metadata metadata)
        {
            var keyNames = metadata.EntityTypes[entityTypeName].Key;
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