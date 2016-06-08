namespace Server.Models.Utils.DAL.Common
{
    public class DataServiceDto : DataServiceBase
    {
        public DataServiceDto(string metadataFileName = "", string connectionString = "")
            : base(metadataFileName, connectionString)
        {
            this.DataViewDto = new DataViewDto(this.DataAdapter);
        }

        public DataViewDto DataViewDto { get; private set; }
    }
}