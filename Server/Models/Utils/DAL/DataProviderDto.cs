namespace Server.Models.Utils.DAL.Common
{
    public static class DataProviderDto
    {
        public static DataServiceDto CreateDataServiceInstance()
        {
            var dataService = new DataServiceDto();
            return dataService;
        }
    }
}