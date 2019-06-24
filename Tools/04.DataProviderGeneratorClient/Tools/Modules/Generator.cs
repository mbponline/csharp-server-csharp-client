using System.Collections.Generic;
using System.Linq;
using Tools.Modules.Common;
using Tools.Modules.Common.MetadataCli;
using MetadataCli = Tools.Modules.Common.MetadataCli;

namespace Tools.Modules
{
    internal static class Generator
    {
        public static string Generate(MetadataCli.Metadata metadataCliFull)
        {
            var entityTypes = metadataCliFull.EntityTypes.ToList();

            var function = metadataCliFull.Functions != null ? metadataCliFull.Functions : Enumerable.Empty<Operation>();
            var action = metadataCliFull.Actions != null ? metadataCliFull.Actions : Enumerable.Empty<Operation>();

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
                .WriteLine("using Client.Modules.Utils.DAL.Common;")
                .WriteLine("using System;")
                .WriteLine("using System.Collections.Generic;")
                .WriteLine("using System.Threading.Tasks;")
                .WriteLine("using MetadataCli = Client.Modules.Utils.DAL.Common.MetadataCli;")
                .WriteLine();

            br.WriteLine("namespace Client.Modules.Utils.DAL");
            br.BeginBlock("{");

            // DataService
            br.WriteLine("public class DataService : DataServiceBase<LocalViews, RemoteViews, ServiceFunctions, ServiceActions>");
            br.BeginBlock("{");

            br.WriteLine("public DataService(string baseUrl, string apiUrl, MetadataCli.Metadata metadataCli) : base(baseUrl, apiUrl, metadataCli)");
            br.BeginBlock("{")
                .WriteLine("this.From = new ServiceLocation<LocalViews, RemoteViews>() { Local = new LocalViews(this.DataContext), Remote = new RemoteViews(this.DataAdapter, this.DataContext, metadataCli) };")
                .WriteLine("this.Operation = new ServiceOperation<ServiceFunctions, ServiceActions>() { Function = new ServiceFunctions(this.DataAdapter, this.DataContext), Action = new ServiceActions(this.DataAdapter, this.DataContext) };");
            br.EndBlock("}", false);

            br.EndBlock("}");

            // ServiceFunctions
            br.WriteLine("public class ServiceFunctions : OperationsProvider");
            br.BeginBlock("{")
                .WriteLine("public ServiceFunctions(DataAdapter dataAdapter, DataContext dataContext) : base(dataAdapter, dataContext) { }")
                .WriteLine();
            GeneratorUtils.WriteServiceOperationsProperties(br, "functions", function);
            br.EndBlock("}");

            // ServiceActions
            br.WriteLine("public class ServiceActions : OperationsProvider");
            br.BeginBlock("{")
                .WriteLine("public ServiceActions(DataAdapter dataAdapter, DataContext dataContext) : base(dataAdapter, dataContext) { }")
                .WriteLine();
            GeneratorUtils.WriteServiceOperationsProperties(br, "actions", action);
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
                .WriteLine("public RemoteViews(DataAdapter dataAdapter, DataContext dataContext, MetadataCli.Metadata metadataCli) : base(dataAdapter, dataContext, metadataCli) { }")
                .WriteLine();
            foreach (var entityType in entityTypes)
            {
                br.WriteLine(string.Format("public DataViewRemote<{0}> {1} {{ get {{ return this.GetPropertyValue<DataViewRemote<{0}>>(); }} }}", entityType.Key, entityType.Value.EntitySetName));
            }
            br.EndBlock("}");

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
                GeneratorUtils.WriteProperties(br, etp);

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
