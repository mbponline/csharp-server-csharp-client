using Client.Modules.Utils.DAL;
using Client.Modules.Utils.DAL.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace ClientTests
{
    [TestClass]
    public class DataAgentTests
    {
        private DataAgent dataAgent;

        [TestInitialize()]
        public async Task Startup()
        {
            this.dataAgent = new DataAgent();
            await this.dataAgent.InitializeAsync();
        }

        [TestMethod]
        public async Task TestInitialization()
        {
            var queryObject = new QueryObject() { Count = true };
            var queryResult = await this.dataAgent.DataService.From.Remote.Categories.GetItemsAsync(queryObject);
            var items = this.dataAgent.DataService.From.Local.Categories.GetItems(it => true);

            Assert.AreEqual(items.Count(), 16);
        }
    }

}
