using Newtonsoft.Json.Linq;
using Server.Models.Utils.DAL.Common;
using System.Collections.Generic;
using System.Web.Http;

namespace Server.Controllers.Dtos
{
    [RoutePrefix("api/datasource")]
    public class CrudController : ApiController
    {
        public CrudController()
        {
            this.dataService = DataProviderDto.CreateDataServiceInstance();
        }

        private readonly DataServiceDto dataService;

        // GET: api/datasource/metadata
        [Route("metadata")]
        [HttpGet]
        public Metadata GetMetadata()
        {
            return this.dataService.MetadataClient;
        }

        // GET: api/datasource/crud/{entitySetName}?skip=20&top=10
        [Route("crud/{entitySetName}")]
        [HttpGet]
        public ResultSerialResponse Get(string entitySetName, [FromUri] QueryParams queryParams)
        {
            return ApiProvider.HandleGet(entitySetName, queryParams, this.dataService);
        }

        // GET: api/datasource/crud/single/{entitySetName}?keys=key1:{key1}
        [Route("crud/single/{entitySetName}")]
        [HttpGet]
        public ResultSingleSerialData GetSingle(string entitySetName, [FromUri] QueryParams queryParams)
        {
            return ApiProvider.HandleGetSingle(entitySetName, queryParams, this.dataService);
        }

        // GET: api/datasource/crud/many/{entitySetName}?keys=key1:1,2,3,4;key2:4,5,6,7
        [Route("crud/many/{entitySetName}")]
        [HttpGet]
        public ResultSerialData GetMany(string entitySetName, [FromUri] QueryParams queryParams)
        {
            return ApiProvider.HandleGetMany(entitySetName, queryParams, this.dataService);
        }

        // PUT: api/datasource/crud/{entitySetName}?keys=key1:{key1}
        [Route("crud/{entitySetName}")]
        [HttpPut]
        public ResultSingleSerialData Put(string entitySetName, [FromUri] QueryParams queryParams, [FromBody] JObject jdto)
        {
            var dto = jdto.ToObject<Dto>();
            return ApiProvider.HandleUpdateEntity(entitySetName, queryParams, dto, this.dataService);
        }

        // PATCH: api/datasource/crud/{entitySetName}?keys=key1:{key1}
        [Route("crud/{entitySetName}")]
        [HttpPatch]
        public ResultSingleSerialData Patch(string entitySetName, [FromUri] QueryParams queryParams, [FromBody] JObject jdto)
        {
            var dto = jdto.ToObject<Dto>();
            return ApiProvider.HandleUpdateEntity(entitySetName, queryParams, dto, this.dataService);
        }

        // POST: api/datasource/crud/{entitySetName}
        [Route("crud/{entitySetName}")]
        [HttpPost]
        public ResultSingleSerialData Post(string entitySetName, [FromBody] JObject jdto)
        {
            var dto = jdto.ToObject<Dto>();
            return ApiProvider.HandleInsertEntity(entitySetName, dto, this.dataService);
        }

        // DELETE: api/datasource/crud/{entitySetName}?keys=key1:{key1}
        [Route("crud/{entitySetName}")]
        [HttpDelete]
        public ResultSingleSerialData Delete(string entitySetName, [FromUri] QueryParams queryParams)
        {
            return ApiProvider.HandleDeleteEntity(entitySetName, queryParams, this.dataService);
        }

        // PUT: api/datasource/crud/batch/{entitySetName}
        [Route("crud/batch/{entitySetName}")]
        [HttpPut]
        public List<ResultSingleSerialData> PutBatch(string entitySetName, [FromBody] JObject[] jdtos)
        {
            var dtos = new List<Dto>();
            foreach (var jdto in jdtos)
            {
                dtos.Add(jdto.ToObject<Dto>());
            }
            return ApiProvider.HandleUpdateEntityBatch(entitySetName, dtos.ToArray(), this.dataService);
        }

        // PATCH: api/datasource/crud/batch/{entitySetName}
        [Route("crud/{entitySetName}")]
        [HttpPatch]
        public List<ResultSingleSerialData> PatchBatch(string entitySetName, [FromBody] JObject[] jdtos)
        {
            var dtos = new List<Dto>();
            foreach (var jdto in jdtos)
            {
                dtos.Add(jdto.ToObject<Dto>());
            }
            return ApiProvider.HandleUpdateEntityBatch(entitySetName, dtos.ToArray(), this.dataService);
        }

        // POST: api/datasource/crud/batch/{entitySetName}
        [Route("crud/{entitySetName}")]
        [HttpPost]
        public List<ResultSingleSerialData> PostBatch(string entitySetName, [FromBody] JObject[] jdtos)
        {
            var dtos = new List<Dto>();
            foreach (var jdto in jdtos)
            {
                dtos.Add(jdto.ToObject<Dto>());
            }
            return ApiProvider.HandleInsertEntityBatch(entitySetName, dtos.ToArray(), this.dataService);
        }

        //// DELETE: api/datasource/crud/batch/{entitySetName}
        //[Route("crud/{entitySetName}")]
        //[HttpDelete]
        //public ResultSerialData DeleteBatch1(string entitySetName, [FromBody] JObject[] jdtos)
        //{
        //    var dtos = new List<Dto>();
        //    foreach (var jdto in jdtos)
        //    {
        //        dtos.Add(jdto.ToObject<Dto>());
        //    }
        //    return ApiProvider.HandleDeleteEntityBatch1(entitySetName, dtos.ToArray(), this.dataService);
        //}

        // DELETE: api/datasource/crud/batch/{entitySetName}?keys=key1:1,2,3,4;key2:4,5,6,7
        [Route("crud/{entitySetName}")]
        [HttpDelete]
        public ResultSerialData DeleteBatch(string entitySetName, [FromUri] QueryParams queryParams)
        {
            return ApiProvider.HandleDeleteEntityBatch(entitySetName, queryParams, this.dataService);
        }
    }
}
