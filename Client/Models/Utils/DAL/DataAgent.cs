using System.Threading.Tasks;

namespace Client.Models.Utils.DAL
{
    class DataAgent
    {

        public DataService DataService { get; private set; }

        public async Task InitializeAsync()
        {
            var baseUrl = "http://localhost:8080/";
            var serviceUrl = "/api/datasource/";
            this.DataService = await DataService.CreateInstanceAsync(baseUrl, serviceUrl);
            await LoadCacheDataAsync();
        }

        private async Task LoadCacheDataAsync()
        {
            await this.DataService.From.Remote.Cities.GetItemsAsync();
        }

    }
}
