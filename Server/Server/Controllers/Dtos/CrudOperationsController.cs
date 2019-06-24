using Microsoft.AspNetCore.Mvc;
using Server.Models.DataAccess;
using NavyBlueDtos;
using System.Linq;

namespace Server.Controllers.Dtos
{
    [Produces("application/json")]
    [Route("api/datasource/operations")]
    public class CrudOperationsController : Controller
    {
        public CrudOperationsController(IDataProviderDto dataProviderDto, IDataProvider dataProvider)
        {
            this.dataServiceDto = dataProviderDto.CreateDataServiceInstance();
            this.dataService = dataProvider.CreateDataServiceInstance();
        }

        private readonly DataServiceDto dataServiceDto;
        private readonly DataService dataService;


        // GET: api/datasource/operations/GetFilmsWithActors?releaseYear=2006
        [Route("GetFilmsWithActors")]
        [HttpGet]
        public ResultSerialResponse GetFilmsWithActors([FromQuery] int releaseYear, [FromQuery] QueryParams queryParams)
        {
            var queryObject = QueryUtils.RenderQueryObject(queryParams);
            queryObject.Filter = string.Format("ReleaseYear='{0}'", releaseYear);

            var resultSerialResponse = this.dataServiceDto.ResultSerialUtils.FetchResponseData("Film", queryObject);
            return resultSerialResponse;
        }

        // GET: api/datasource/operations/GetFilmsWithActors1?releaseYear=2006
        [Route("GetFilmsWithActors1")]
        [HttpGet]
        public ResultSerialData GetFilmsWithActors1([FromQuery] int releaseYear, [FromQuery] QueryParams queryParams)
        {
            var queryObject = QueryUtils.RenderQueryObject(queryParams);
            queryObject.Filter = string.Format("ReleaseYear='{0}'", releaseYear);

            var resultSerialData1 = this.dataService.From.Remote.DtoView.Films.GetItems(queryObject);
            var entities1 = this.dataService.From.Remote.EntityView.Films.GetItems(queryObject);
            var entities2 = this.dataService.From.Remote.EntityView.Films.GetItems(queryObject);
            //var derivedEntities = entities1.Select(it => new Film(it));

            foreach (var it in entities1)
            {
                var test0 = it.FilmId;
                var test1 = it.FilmActors;
                if (test1.Any())
                {
                    var test2 = test1.FirstOrDefault();
                    var test3 = test2.Actor;
                }
                var test4 = it.FilmCategories;
            }

            var entities3 = this.dataService.From.Local.EntityView.Films.GetItems(it => true); //.Select(it => new Film(it));
            var resultSerialData2 = this.dataService.From.Local.DtoView.Films.GetItems(it => true, queryObject.Expand);
            return resultSerialData2;
        }

        // POST: api/datasource/operations/TestAction
        // with: Content-Type: application/json and body - {"param1":1}
        [Route("TestAction")]
        [HttpPost]
        public void TestAction([FromBody] dynamic body)
        {
            var param1 = (int)body.param1;
            // TODO: Add some actions in here
        }

    }

}
