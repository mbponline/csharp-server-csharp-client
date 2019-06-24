using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using MetadataCli = Tools.Modules.Common.MetadataCli;

namespace Tools.Modules
{
    public static class Generator
    {
        public static MetadataCli.Metadata GenerateMetadataCliFull(MetadataCli.Metadata metadataSrv, MetadataCli.Metadata metadataCliOperations)
        {
            MetadataCli.Metadata metadataCliFull = null;

            var dbTypeConvert = GeneratorUtils.GetTypeConvert(metadataSrv.Dialect);

            // Concateneaza toate entityTypes-urile

            var entityTypes = new Dictionary<string, MetadataCli.EntityType>();

            metadataSrv.EntityTypes.ConvertPropertyTypes(dbTypeConvert);
            foreach (var item in metadataSrv.EntityTypes)
            {
                var entityTypeName = item.Key;
                var entityType = item.Value;
                entityType.AddAnnotation("IsQueryable", true);
                entityType.AddAnnotation("IsVirtual", false);
                entityType.AddAnnotation("IdGroup", 1);
                entityTypes.Add(entityTypeName, entityType);
            }

            metadataCliFull = new MetadataCli.Metadata
            {
                Dialect = "CS",
                Version = "v0.0.1",
                Description = "Sakila client metadata, full version (used also by tools)",
                Max = 256,
                Multiplicity = new MetadataCli.Multiplicity()
                {
                    Multi = "multi",
                    Single = "single"
                },
                EntityTypes = entityTypes,
                Functions = metadataCliOperations.Functions,
                Actions = metadataCliOperations.Actions
            };

            return metadataCliFull;
        }

        public static JObject GenerateMetadataCli(MetadataCli.Metadata metadataCliFull)
        {
            if (metadataCliFull.Dialect != "CS")
            {
                throw new ArgumentException("Unknown dialect");
            }

            var metadataCli = JObject.FromObject(metadataCliFull).ToObject<MetadataCli.Metadata>();

            var opTypeConvert = new Dictionary<string, string>()
                {
                    { "int", "number" },
                    { "short", "number" },
                    { "ushort", "number" },
                    { "byte", "number" },
                    { "sbyte", "number" },
                    { "float", "number" },
                    { "DateTime", "Date" },
                    { "string", "string" },
                    { "bool", "boolean" },
                    { "object", "any" },
                    { "byte[]", "any" }
                };

            metadataCli.EntityTypes.ConvertPropertyTypes(opTypeConvert);

            metadataCli.Functions.ConvertOperationType(opTypeConvert);
            metadataCli.Actions.ConvertOperationType(opTypeConvert);

            metadataCli.Dialect = "TS";
            metadataCli.Description = "Sakila client metadata";

            //return metadataCli;

            var metadataCliJson = JObject.FromObject(metadataCli);

            metadataCliJson.Remove("namespace");
            //metadataCliJson.Remove("max");

            var entityTypes = metadataCliJson["entityTypes"].Value<JObject>();

            foreach (var et in entityTypes)
            {
                var entityTypeName = et.Key;
                var entityType = et.Value.Value<JObject>();
                entityType.Remove("tableName");
                entityType.Remove("annotations");

                var properties = entityType["properties"].Value<JObject>();
                foreach (var prop in properties)
                {
                    var propertyName = prop.Key;
                    var property = prop.Value.Value<JObject>();
                    property.Remove("annotations");
                }

                var navigationProperties = entityType["navigationProperties"].Value<JObject>();
                foreach (var prop in navigationProperties)
                {
                    var navigationPropertyName = prop.Key;
                    var navigationProperty = prop.Value.Value<JObject>();
                    navigationProperty.Remove("annotations");
                }

            }

            return metadataCliJson;
        }

    }

}
