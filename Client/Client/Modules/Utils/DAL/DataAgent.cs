using Client.Modules.Utils.DAL.Common;
using System.Threading.Tasks;

namespace Client.Modules.Utils.DAL
{
    public class DataAgent
    {

        public DataService DataService { get; private set; }

        public async Task InitializeAsync()
        {
            var baseUrl = "http://localhost:50069";
            var apiUrl = "/api/datasource";
            var metadataCli = await MetadataUtils.GetMetadataAsync(baseUrl, "/api/datasource/metadata");
            this.DataService = new DataService(baseUrl, apiUrl, metadataCli);
            await LoadCacheDataAsync();
        }

        private async Task LoadCacheDataAsync()
        {
            await this.DataService.From.Remote.Categories.GetItemsAsync();
        }

    }
}
