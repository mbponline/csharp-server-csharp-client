
namespace NavyBlueDtos
{
    public class DataServiceDto
    {
        public MetadataSrv.Metadata MetadataSrv { get; private set; }
        public DataViewDto DataViewDto { get; private set; }
        public ApiProviderDto ApiProviderDto { get; private set; }
        public ResultSerialUtils ResultSerialUtils { get; private set; }

        public DataServiceDto(string connectionString, MetadataSrv.Metadata metadataSrv)
        {
            this.MetadataSrv = metadataSrv;

            var dialect = metadataSrv.Dialect();
            var databaseOperations = new DatabaseOperations(connectionString, dialect);
            var dataAdapterRead = new DataAdapterRead(databaseOperations, dialect, metadataSrv);
            var dataAdapterCud = new DataAdapterCud(dataAdapterRead, databaseOperations, dialect, metadataSrv);
            this.DataViewDto = new DataViewDto(dataAdapterRead, dataAdapterCud, metadataSrv);
            this.ResultSerialUtils = new ResultSerialUtils(this.DataViewDto, "crud", metadataSrv);
            this.ApiProviderDto = new ApiProviderDto(this.DataViewDto, this.ResultSerialUtils, metadataSrv);
        }

    }

}