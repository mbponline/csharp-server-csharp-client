using Client.Modules.Utils.DAL;
using Client.Modules.Utils.DAL.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace ClientTests
{
    [TestClass]
    public class DataServiceTests
    {
        private DataService dataService;

        [TestInitialize()]
        public async Task Startup()
        {
            var baseUrl = "http://localhost:50069";
            var apiUrl = "/api/datasource";
            var metadataCli = await MetadataUtils.GetMetadataAsync(baseUrl, "/api/datasource/metadata");
            this.dataService = new DataService(baseUrl, apiUrl, metadataCli);
        }

        [TestMethod]
        public async Task TestGetLoturi()
        {
            var queryObject = new QueryObject() { Count = true };
            var queryResult = await this.dataService.From.Remote.Categories.GetItemsAsync(queryObject);

            Assert.AreEqual(queryResult.Rows.Count(), 16);
            Assert.AreEqual(queryResult.TotalRows, 16);
        }
    }

}
