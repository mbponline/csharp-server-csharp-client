using Server.Models.Utils.DAL.Common;
using System.Web.Http;

namespace Server.Controllers.Dtos
{
    [RoutePrefix("api/datasource/operations")]
    public class CrudOperationsController : ApiController
    {
        public CrudOperationsController()
        {
            this.dataServiceDto = new DataServiceDto();
        }

        private readonly DataServiceDto dataServiceDto;

        // GET: api/datasource/operations/GetFilmsWithActors?releaseYear=2006
        [Route("GetFilmsWithActors")]
        [HttpGet]
        public ResultSerialResponse Get([FromUri] int releaseYear)
        {
            //var expand = NavigationHelper<Film>.Get()
            //    .Include((it) => it.FilmActors.Select().Actor)
            //    .Include((it) => it.FilmCategories).All();

            var queryObject = new QueryObject()
            {
                Filter = string.Format("ReleaseYear='{0}'", releaseYear),
                Expand = new string[] { "FilmActors.Actor", "FilmCategories.Category" }
            };

            var films = this.dataServiceDto.DataViewDto.GetItems("Film", queryObject);
            return ResultSerialUtils.FetchResponseData("Film", queryObject, "operations", this.dataServiceDto);
        }

        // POST: api/datasource/operations/TestAction
        // with: Content-Type: application/json and body - {"param1":1}
        [Route("TestAction")]
        [HttpPost]
        public void Post([FromBody] int param1)
        {
            // TODO: Add some actions in here
        }
    }
}
