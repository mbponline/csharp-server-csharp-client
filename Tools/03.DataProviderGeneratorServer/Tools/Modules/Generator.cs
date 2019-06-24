using System;
using System.Collections.Generic;
using System.Linq;
using Tools.Modules.Common;
using MetadataCli = Tools.Modules.Common.MetadataCli;

namespace Tools.Modules
{
    internal static class Generator
    {
        public static string Generate(MetadataCli.Metadata metadataCli)
        {
            var entityTypes = metadataCli.EntityTypes.ToList();
            var entitySets = (from t in metadataCli.EntityTypes select new { name = t.Value.EntitySetName, entityTypeName = t.Key }).ToList();

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

            br.WriteLine("using NavyBlueDtos;");
            br.WriteLine("using NavyBlueEntities;");
            br.WriteLine("using Newtonsoft.Json;");
            br.WriteLine("using Newtonsoft.Json.Linq;");
            br.WriteLine("using System;");
            br.WriteLine("using System.Collections.Generic;");
            br.WriteLine("using MetadataSrv = NavyBlueDtos.MetadataSrv;");
            br.WriteLine();

            br.WriteLine("namespace Server.Models.DataAccess");
            br.BeginBlock("{");

            // DataProvider
            br.WriteLine("public interface IDataProvider");
            br.BeginBlock("{")
                .WriteLine("DataService CreateDataServiceInstance();");
            br.EndBlock("}");

            br.WriteLine("public class DataProvider : IDataProvider");
            br.BeginBlock("{")
                .WriteLine("private readonly IDataProviderDto dataProviderDto;")
                .WriteLine();

            br.WriteLine("public DataProvider(IDataProviderDto dataProviderDto)");
            br.BeginBlock("{")
                .WriteLine("this.dataProviderDto = dataProviderDto;");
            br.EndBlock("}");

            br.WriteLine("public DataService CreateDataServiceInstance()");
            br.BeginBlock("{")
                .WriteLine("var dataServiceDto = this.dataProviderDto.CreateDataServiceInstance();")
                .WriteLine("var dataService = new DataService(dataServiceDto);")
                .WriteLine("return dataService;");
            br.EndBlock("}", false);

            br.EndBlock("}");

            // DataService
            br.WriteLine("public class DataService : DataServiceEntity<LocalEntityViews, LocalDtoViews, RemoteEntityViews, RemoteDtoViews>");
            br.BeginBlock("{");
            br.WriteLine("public DataService(DataServiceDto dataServiceDto) : base(dataServiceDto)");
            br.BeginBlock("{");
            br.WriteLine("this.From = new ServiceLocation<LocalEntityViews, LocalDtoViews, RemoteEntityViews, RemoteDtoViews>()");
            br.BeginBlock("{")
                .WriteLine("Local = new ViewType<LocalEntityViews, LocalDtoViews>() { EntityView = new LocalEntityViews(this.DataContext), DtoView = new LocalDtoViews(this.DataContext, dataServiceDto.MetadataSrv) },")
                .WriteLine("Remote = new ViewType<RemoteEntityViews, RemoteDtoViews>() { EntityView = new RemoteEntityViews(dataServiceDto.DataViewDto, this.DataContext), DtoView = new RemoteDtoViews(dataServiceDto.DataViewDto) }");
            br.EndBlock("};", false);
            br.EndBlock("}", false);

            br.EndBlock("}");

            // LocalEntityViews
            br.WriteLine("public class LocalEntityViews : LocalEntityViewsBase");
            br.BeginBlock("{");
            br.WriteLine("public LocalEntityViews(DataContext dataContext) : base(dataContext)");
            br.BeginBlock("{");
            foreach (var es in entitySets)
            {
                br.WriteLine(string.Format("//this.[\"{0}\"] = new DataViewLocalEntity<{1}>(dataContext);", es.name, es.entityTypeName));
            }
            br.EndBlock("}");
            foreach (var es in entitySets)
            {
                br.WriteLine(string.Format("public DataViewLocalEntity<{1}> {0} {{ get {{ return this.GetPropertyValue<{1}>(); }} }}", es.name, es.entityTypeName));
            }
            br.EndBlock("}");

            // RemoteEntityViews
            br.WriteLine("public class RemoteEntityViews : RemoteEntityViewsBase");
            br.BeginBlock("{");
            br.WriteLine("public RemoteEntityViews(DataViewDto dataViewDto, DataContext dataContext) : base(dataViewDto, dataContext)");
            br.BeginBlock("{");
            foreach (var es in entitySets)
            {
                br.WriteLine(string.Format("//this.[\"{0}\"] = new DataViewRemoteEntity<{1}>(dataViewDto, dataContext);", es.name, es.entityTypeName));
            }
            br.EndBlock("}");
            foreach (var es in entitySets)
            {
                br.WriteLine(string.Format("public DataViewRemoteEntity<{1}> {0} {{ get {{ return this.GetPropertyValue<{1}>(); }} }}", es.name, es.entityTypeName));
            }
            br.EndBlock("}");

            // LocalDtoViews
            br.WriteLine("public class LocalDtoViews : LocalDtoViewsBase");
            br.BeginBlock("{");
            br.WriteLine("public LocalDtoViews(DataContext dataContext, MetadataSrv.Metadata metadataSrv) : base(dataContext, metadataSrv)");
            br.BeginBlock("{");
            foreach (var es in entitySets)
            {
                br.WriteLine(string.Format("//this.[\"{0}\"] = new DataViewLocalDto<{1}>(dataContext, metadataSrv);", es.name, es.entityTypeName));
            }
            br.EndBlock("}");
            foreach (var es in entitySets)
            {
                br.WriteLine(string.Format("public DataViewLocalDto<{1}> {0} {{ get {{ return this.GetPropertyValue<{1}>(); }} }}", es.name, es.entityTypeName));
            }
            br.EndBlock("}");

            // RemoteDtoViews
            br.WriteLine("public class RemoteDtoViews : RemoteDtoViewsBase");
            br.BeginBlock("{");
            br.WriteLine("public RemoteDtoViews(DataViewDto dataViewDto) : base(dataViewDto)");
            br.BeginBlock("{");
            foreach (var es in entitySets)
            {
                br.WriteLine(string.Format("//this.[\"{0}\"] = new DataViewRemoteDto<{1}>(dataViewDto);", es.name, es.entityTypeName));
            }
            br.EndBlock("}");
            foreach (var es in entitySets)
            {
                br.WriteLine(string.Format("public DataViewRemoteDto {0} {{ get {{ return this.GetPropertyValue(\"{1}\"); }} }}", es.name, es.entityTypeName));
            }
            br.EndBlock("}");

            // Entities
            foreach (var et in entityTypes)
            {
                var entityTypeName = et.Key;
                var properties = et.Value.Properties;
                var navigationProperties = et.Value.NavigationProperties ?? new Dictionary<string, MetadataCli.NavigationProperty>();

                // with constructor generator
                br.WriteLine(string.Format("public sealed class {0} : IDerivedEntity", entityTypeName));
                br.BeginBlock("{");
                br.WriteLine(string.Format("public {0}(Entity entity)", entityTypeName));
                br.BeginBlock("{")
                  .WriteLine(string.Format("if (entity.entityTypeName != \"{0}\") {{ throw new ArgumentException(\"Incorrect entity type\"); }}", entityTypeName))
                  .WriteLine("this.entity = entity;");
                br.EndBlock("}");

                br.WriteLine("public Entity entity { get; private set; }")
                  .WriteLine();

                GeneratorUtils.WriteProperties(br, properties);

                // navigation properties for intellisense
                GeneratorUtils.WriteNavigationProperties(br, entityTypeName, navigationProperties);

                br.EndBlock("}");
            }

            br.EndBlock("}");

            br.WriteLine("#pragma warning restore SA1649, SA1128, SA1005, SA1516, SA1402, SA1028, SA1119, SA1507, SA1502, SA1508, SA1122, SA1633, SA1300");

            return br.ToString();
        }
    }

}
