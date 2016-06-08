using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace Server.Models.Utils.DAL.Common
{
    public static class MetadataUtils
    {
        public static Metadata Deserialize(string metadataFileName)
        {
            Metadata metadata;
            OperationsDefinition operationsDefinition;

            // read json files
            var pathMetadata = HostingEnvironment.MapPath(string.Format(@"~/App_Data/{0}.json", metadataFileName));
            using (StreamReader r = new StreamReader(pathMetadata))
            {
                var jsonText = r.ReadToEnd();
                metadata = JsonConvert.DeserializeObject<Metadata>(jsonText);
            }

            var pathOperationsDefinition = HostingEnvironment.MapPath(@"~/App_Data/operationsDefinition.json");
            using (StreamReader r = new StreamReader(pathOperationsDefinition))
            {
                var jsonText = r.ReadToEnd();
                operationsDefinition = JsonConvert.DeserializeObject<OperationsDefinition>(jsonText);
            }

            metadata.Functions = operationsDefinition.Functions;
            metadata.Actions = operationsDefinition.Actions;

            return metadata;
        }

        public static Metadata ToMetadataClient(this Metadata metadata)
        {
            Dictionary<string, string> dbTypeConvert = null;

            switch (metadata.Dialect())
            {
                case Dialect.SQL2012:
                case Dialect.SQL2014:
                    dbTypeConvert = new Dictionary<string, string>()
                        {
                            { "int", "number" },
                            { "smallint", "number" },
                            { "real", "number" },
                            { "datetime", "Date" },
                            { "nvarchar", "string" },
                            { "text", "string" },
                            { "bit", "boolean" }
                        };
                    break;
                case Dialect.MYSQL:
                    dbTypeConvert = new Dictionary<string, string>()
                        {
                            { "int", "number" },
                            { "smallint", "number" },
                            { "float", "number" },
                            { "decimal", "number" },
                            { "mediumint", "number" },
                            { "tinyint", "number" },
                            { "datetime", "Date" },
                            { "timestamp", "Date" },
                            { "bit", "boolean" },
                            { "char", "string" },
                            { "varchar", "string" },
                            { "text", "string" },
                            { "longtext", "string" },
                            { "enum", "string" },
                            { "set", "string" },
                            { "geometry", "any" },
                            { "year", "number" },
                            { "blob", "any" },
                        };
                    break;
            }

            var opTypeConvert = new Dictionary<string, string>()
                        {
                            { "int", "number" },
                            { "DateTime", "Date" },
                            { "string", "string" },
                            { "bool", "boolean" }
                        };

            // clone metadata
            var metadataClient = JObject.FromObject(metadata).ToObject<Metadata>();

            //metadataClient.TinyintAsBoolean = tinyintAsBoolean;

            foreach (var entityTypeName in metadataClient.EntityTypes)
            {
                var properties = metadataClient.EntityTypes[entityTypeName.Key].Properties;
                foreach (var property in properties)
                {
                    property.Value.Type = dbTypeConvert[property.Value.Type];
                }
            }

            foreach (var function in metadataClient.Functions)
            {
                ConvertOperationType(function, opTypeConvert);
            }

            foreach (var action in metadataClient.Actions)
            {
                ConvertOperationType(action, opTypeConvert);
            }

            return metadataClient;
        }

        private static void ConvertOperationType(Operation operation, Dictionary<string, string> opTypeConvert)
        {
            foreach (var parameter in operation.Parameters)
            {
                parameter.Type = opTypeConvert[parameter.Type];
            }
            if (operation.ReturnType != null && !operation.ReturnType.IsEntity)
            {
                operation.ReturnType.Type = opTypeConvert[operation.ReturnType.Type];
            }
        }

    }
}