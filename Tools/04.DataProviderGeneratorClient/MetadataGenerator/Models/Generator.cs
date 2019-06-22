using CodeGenerator.Models.Common;
using System.Collections.Generic;
using System.Linq;

namespace MetadataGenerator.Models
{
    internal static class Generator
    {
        public static string GenerateModel(Metadata metadata, OperationsDefinition operationsDefinition)
        {
            metadata.Functions = operationsDefinition.Functions;
            metadata.Actions = operationsDefinition.Actions;

            var entityTypes = metadata.EntityTypes.ToList();

            var function = metadata.Functions != null ? metadata.Functions : Enumerable.Empty<Operation>();
            var action = metadata.Actions != null ? metadata.Actions : Enumerable.Empty<Operation>();

            Dictionary<string, string> dbTypeConvert = null;

            switch (metadata.Database.Dialect)
            {
                case "MSSQL":
                    dbTypeConvert = new Dictionary<string, string>()
                        {
                            { "int", "int" },
                            { "smallint", "short" },
                            { "real", "float" },
                            { "datetime", "DateTime" },
                            { "nvarchar", "string" },
                            { "text", "string" },
                            { "bit", "bool" }
                        };
                    break;
                case "MYSQL":
                    dbTypeConvert = new Dictionary<string, string>()
                        {
                            { "int", "int" },
                            { "smallint", "short" },
                            { "float", "float" },
                            { "decimal", "float" },
                            { "mediumint", "int" },
                            { "tinyint", "sbyte" },
                            { "datetime", "DateTime" },
                            { "timestamp", "DateTime" },
                            { "bit", "bool" },
                            { "char", "string" },
                            { "varchar", "string" },
                            { "text", "string" },
                            { "longtext", "string" },
                            { "enum", "string" },
                            { "set", "string" },
                            { "geometry", "object" },
                            { "year", "ushort" },
                            { "blob", "byte[]" },
                        };
                    break;
                default:
                    break;
            }

            var opTypeConvert = new Dictionary<string, string>()
                {
                    { "int", "int" },
                    { "DateTime", "DateTime" },
                    { "string", "string" },
                    { "bool", "bool" }
                };

            var br = new BlockWriter();

            br.WriteLine("#pragma warning disable SA1649, SA1128, SA1005, SA1516, SA1402, SA1028, SA1119, SA1507, SA1502, SA1508, SA1122, SA1633, SA1300")
                .WriteLine()
                .WriteLine("//------------------------------------------------------------------------------")
                .WriteLine("//    This code was auto-generated.")
                .WriteLine("//")
                .WriteLine("//    Manual changes to this file may cause unexpected behavior in your application.")
                .WriteLine("//    Manual changes to this file will be overwritten if the code is regenerated.")
                .WriteLine("//------------------------------------------------------------------------------")
                .WriteLine();

            br.WriteLine("using Newtonsoft.Json;")
                .WriteLine("using Client.Models.Utils.DAL.Common;")
                .WriteLine("using System;")
                .WriteLine("using System.Collections.Generic;")
                .WriteLine("using System.Threading.Tasks;")
                .WriteLine();

            br.WriteLine("namespace Client.Models.Utils.DAL");
            br.BeginBlock("{");

            // DataService
            br.WriteLine("public class DataService : DataServiceBase<LocalViews, RemoteViews, ServiceFunctions, ServiceActions>");
            br.BeginBlock("{");

            br.WriteLine("public DataService(Metadata metadata, string baseUrl, string serviceUrl) : base(metadata, baseUrl, serviceUrl)");
            br.BeginBlock("{")
                .WriteLine("this.From = new ServiceLocation<LocalViews, RemoteViews>() { Local = new LocalViews(this.DataContext), Remote = new RemoteViews(this.DataAdapter, this.DataContext, metadata) };")
                .WriteLine("this.Operation = new ServiceOperation<ServiceFunctions, ServiceActions>() { Function = new ServiceFunctions(this.DataAdapter, this.DataContext, metadata), Action = new ServiceActions(this.DataAdapter, this.DataContext, metadata) };");
            br.EndBlock("}");

            br.WriteLine("public static async Task<DataService> CreateInstanceAsync(string baseUrl, string serviceUrl)");
            br.BeginBlock("{")
                .WriteLine("var metadata = await DataService.GetMetadataAsync(baseUrl, serviceUrl);")
                .WriteLine("metadata.Namespace = \"Client.Models.Utils.DAL\";")
                .WriteLine("return new DataService(metadata, baseUrl, serviceUrl);");
            br.EndBlock("}");

            br.EndBlock("}");

            // ServiceFunctions
            br.WriteLine("public class ServiceFunctions : OperationsProvider");
            br.BeginBlock("{")
                .WriteLine("public ServiceFunctions(DataAdapter dataAdapter, DataContext dataContext, Metadata metadata) : base(dataAdapter, dataContext, metadata) { }")
                .WriteLine();
            GeneratorUtils.WriteServiceOperationsProperties(br,"functions", function, opTypeConvert);
            br.EndBlock("}");

            // ServiceActions
            br.WriteLine("public class ServiceActions : OperationsProvider");
            br.BeginBlock("{")
                .WriteLine("public ServiceActions(DataAdapter dataAdapter, DataContext dataContext, Metadata metadata) : base(dataAdapter, dataContext, metadata) { }")
                .WriteLine();
            GeneratorUtils.WriteServiceOperationsProperties(br, "actions", action, opTypeConvert);
            br.EndBlock("}");

            // LocalViews
            br.WriteLine("public class LocalViews : PropertyList");
            br.BeginBlock("{")
                .WriteLine("public LocalViews(DataContext dataContext) : base(dataContext) { }")
                .WriteLine();
            foreach (var entityType in entityTypes)
            {
                br.WriteLine(string.Format("public DataViewLocal<{0}> {1} {{ get {{ return this.GetPropertyValue<DataViewLocal<{0}>>(); }} }}", entityType.Key, entityType.Value.EntitySetName));
            }
            br.EndBlock("}");

            // RemoteViews
            br.WriteLine("public class RemoteViews : PropertyList");
            br.BeginBlock("{")
                .WriteLine("public RemoteViews(DataAdapter dataAdapter, DataContext dataContext, Metadata metadata) : base(dataAdapter, dataContext, metadata) { }")
                .WriteLine();
            foreach (var entityType in entityTypes)
            {
                br.WriteLine(string.Format("public DataViewRemote<{0}> {1} {{ get {{ return this.GetPropertyValue<DataViewRemote<{0}>>(); }} }}", entityType.Key, entityType.Value.EntitySetName));
            }
            br.EndBlock("}");

            //// entityTypes
            //br.BeginBlock("export var entityTypes = {");
            //foreach (var entityType in entityTypes)
            //{
            //    br.WriteLine(string.Format("{0}: '{0}',", entityType.Key));
            //}
            //br.EndBlock("}");

            // Entities
            foreach (var et in entityTypes)
            {
                var etp = et.Value.Properties;
                var etnp = et.Value.NavigationProperties ?? new Dictionary<string, NavigationProperty>();

                // constructor generator
                br.WriteLine(string.Format("public sealed class {0} : IDerivedEntity", et.Key));
                br.BeginBlock("{");
                br.WriteLine(string.Format("public {0}(Entity entity)", et.Key));
                br.BeginBlock("{");
				br.WriteLine(string.Format("if (entity.entityTypeName != \"{0}\") {{ throw new ArgumentException(\"Incorrect entity type\"); }}", et.Key));
				br.WriteLine("this.entity = entity;");
                br.EndBlock("}");

				br.WriteLine("public Entity entity { get; private set; }")
				  .WriteLine();

                // properties
                GeneratorUtils.WriteProperties(br, etp, dbTypeConvert);

                // navigation properties
                GeneratorUtils.WriteNavigationProperties(br, et.Key, etnp);

                br.EndBlock("}");
            }

            br.EndBlock("}");

            br.WriteLine("#pragma warning restore SA1649, SA1128, SA1005, SA1516, SA1402, SA1028, SA1119, SA1507, SA1502, SA1508, SA1122, SA1633, SA1300");

            return br.ToString();
        }
    }

}
