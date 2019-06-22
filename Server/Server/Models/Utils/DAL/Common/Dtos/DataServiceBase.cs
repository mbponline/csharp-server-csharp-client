using System.Configuration;

namespace Server.Models.Utils.DAL.Common
{
    public abstract class DataServiceBase
    {
        protected DataServiceBase(string metadataFileName, string connectionString)
        {
            if (string.IsNullOrEmpty(metadataFileName))
            {
                metadataFileName = "metadata_mysql";
            }
            this.Metadata = MetadataUtils.Deserialize(metadataFileName);
            this.MetadataClient = this.Metadata.ToMetadataClient();

            // in versiunea finala metadata va fi generata direct din baza de date
            //var connectionString = @"Data Source=.\SQLEXPRESS2012;Initial Catalog=QualityControlDb04;Integrated Security=True;MultipleActiveResultSets=True";
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
            this.DataAdapter = new DataAdapter(this.Metadata, connectionString);
        }

        public Metadata Metadata { get; private set; }

        public Metadata MetadataClient { get; private set; }

        protected DataAdapter DataAdapter { get; private set; }
    }

}