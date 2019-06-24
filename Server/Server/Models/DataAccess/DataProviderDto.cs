using NavyBlueDtos;

namespace Server.Models.DataAccess
{

    public interface IDataProviderDto
    {
        DataServiceDto CreateDataServiceInstance();
    }

    public class DataProviderDto : IDataProviderDto
    {
        private readonly IDataProviderConfig dataProviderConfig;

        public DataProviderDto(IDataProviderConfig dataProviderConfig)
        {
            this.dataProviderConfig = dataProviderConfig;
        }

        public DataServiceDto CreateDataServiceInstance()
        {
            var connectionString = this.dataProviderConfig.GetConnectionString();
            var metadataSrv = this.dataProviderConfig.GetMetadataSrv();
            var dataServiceDto = new DataServiceDto(connectionString, metadataSrv);
            return dataServiceDto;
        }
    }
}