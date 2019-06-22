using CodeGenerator.Models.Common;
using System.Collections.Generic;
using System.Linq;

namespace Tools.Models
{
    internal static class Generator
    {
        public static string GenerateModel(Metadata metadata)
        {
            var entityTypes = metadata.EntityTypes.ToList();
            var entitySets = (from t in metadata.EntityTypes select new { name = t.Value.EntitySetName, entityTypeName = t.Key }).ToList();

            Dictionary<string, string> types = null;

            switch (metadata.Database.Dialect)
            {
                case "MSSQL":
                    types = new Dictionary<string, string>()
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
                    types = new Dictionary<string, string>()
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

            br.WriteLine("using Newtonsoft.Json;");
            br.WriteLine("using Server.Models.Utils.DAL.Common;");
            br.WriteLine("using System;");
            br.WriteLine("using System.Collections.Generic;");
            br.WriteLine();

            br.WriteLine("namespace " + metadata.Namespace);
            br.BeginBlock("{");

            // DataService
            br.WriteLine("public class DataService : DataServiceEntity<LocalEntityViews, LocalDtoViews, RemoteEntityViews, RemoteDtoViews>");
            br.BeginBlock("{");
            br.WriteLine("public DataService(string metadataFileName = \"\", string connectionString = \"\") : base(metadataFileName, connectionString)");
            br.BeginBlock("{");
            br.WriteLine("this.From = new ServiceLocation<LocalEntityViews, LocalDtoViews, RemoteEntityViews, RemoteDtoViews>()");
            br.BeginBlock("{")
                .WriteLine("Local = new ViewType<LocalEntityViews, LocalDtoViews>() { EntityView = new LocalEntityViews(this.DataContext), DtoView = new LocalDtoViews(this.DataContext, this.Metadata) },")
                .WriteLine("Remote = new ViewType<RemoteEntityViews, RemoteDtoViews>() { EntityView = new RemoteEntityViews(this.DataAdapter, this.DataContext), DtoView = new RemoteDtoViews(this.DataAdapter) }");
            br.EndBlock("};", false);
            br.EndBlock("}", false);
            br.EndBlock("}");

            // LocalEntityViews
            br.WriteLine("public class LocalEntityViews : PropertyList");
            br.BeginBlock("{");
            br.WriteLine("public LocalEntityViews(DataContext dataContext) : base(dataContext) { }")
                .WriteLine();
            foreach (var es in entitySets)
            {
                br.WriteLine(string.Format("public DataViewLocalEntity<{0}> {1} {{ get {{ return this.GetPropertyValue<DataViewLocalEntity<{0}>>(); }} }}", es.entityTypeName, es.name));
            }
            br.EndBlock("}");

            // RemoteEntityViews
            br.WriteLine("public class RemoteEntityViews : PropertyList");
            br.BeginBlock("{");
            br.WriteLine("public RemoteEntityViews(DataAdapter dataAdapter, DataContext dataContext) : base(dataAdapter, dataContext) { }")
                .WriteLine();
            foreach (var es in entitySets)
            {
                br.WriteLine(string.Format("public DataViewRemoteEntity<{0}> {1} {{ get {{ return this.GetPropertyValue<DataViewRemoteEntity<{0}>>(); }} }}", es.entityTypeName, es.name));
            }
            br.EndBlock("}");

            // LocalDtoViews
            br.WriteLine("public class LocalDtoViews : PropertyList");
            br.BeginBlock("{");
            br.WriteLine("public LocalDtoViews(DataContext dataContext, Metadata metadata) : base(dataContext, metadata) { }")
                .WriteLine();
            foreach (var es in entitySets)
            {
                br.WriteLine(string.Format("public DataViewLocalDto<{0}> {1} {{ get {{ return this.GetPropertyValue<DataViewLocalDto<{0}>>(); }} }}", es.entityTypeName, es.name));
            }
            br.EndBlock("}");

            // RemoteDtoViews
            br.WriteLine("public class RemoteDtoViews : PropertyList");
            br.BeginBlock("{");
            br.WriteLine("public RemoteDtoViews(DataAdapter dataAdapter) : base(dataAdapter) { }")
                .WriteLine();
            foreach (var es in entitySets)
            {
                br.WriteLine(string.Format("public DataViewRemoteDto<{0}> {1} {{ get {{ return this.GetPropertyValue<DataViewRemoteDto<{0}>>(); }} }}", es.entityTypeName, es.name));
            }
            br.EndBlock("}");

            // Entities
            foreach (var et in entityTypes)
            {
                var etp = et.Value.Properties;
                var etnp = et.Value.NavigationProperties ?? new Dictionary<string, NavigationProperty>();

                // with constructor generator
                br.WriteLine(string.Format("public sealed class {0} : Entity", et.Key));
                br.BeginBlock("{");
                br.WriteLine(string.Format("public {0}() : base()", et.Key));
                br.BeginBlock("{");
                GeneratorUtils.WriteDefaultValues(br, etp);
                br.EndBlock("}");

                GeneratorUtils.WriteProperties(br, etp, types);

                // navigation properties for intellisense
                GeneratorUtils.WriteNavigationProperties(br, et.Key, etnp);

                br.EndBlock("}");
            }

            br.EndBlock("}");

            br.WriteLine("#pragma warning restore SA1649, SA1128, SA1005, SA1516, SA1402, SA1028, SA1119, SA1507, SA1502, SA1508, SA1122, SA1633, SA1300");

            return br.ToString();
        }
    }

}
