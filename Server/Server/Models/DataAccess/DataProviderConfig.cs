using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using MetadataSrv = NavyBlueDtos.MetadataSrv;

namespace Server.Models.DataAccess
{
    public interface IDataProviderConfig
    {
        string GetConnectionString();
        MetadataSrv.Metadata GetMetadataSrv();
    }

    public class DataProviderConfig : IDataProviderConfig
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;

        public DataProviderConfig(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
        }

        public string GetConnectionString()
        {
            // var connectionString = "Server=localhost;Database=qualityControldb12;Uid=root;Pwd=Pass@word1;";
            var connectionString = this.configuration.GetValue<string>("App:DefaultConnection");
            return connectionString;
        }

        public MetadataSrv.Metadata GetMetadataSrv()
        {
            var pathMetadataRelative = this.configuration.GetValue<string>("App:AppData");
            var pathMetadata = Path.Combine(this.hostingEnvironment.ContentRootPath + pathMetadataRelative, "metadata_srv.json");
            var metadataJsonText = File.ReadAllText(pathMetadata);
            var metadataSrv = JsonConvert.DeserializeObject<MetadataSrv.Metadata>(metadataJsonText);
            return metadataSrv;
        }

    }

}