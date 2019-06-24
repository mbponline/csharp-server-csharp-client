using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Server.Models.DataAccess;
using NavyBlueDtos;

namespace Server.Controllers.Dtos
{
    [Produces("application/json")]
    [Route("api/datasource/crud")]
    public class CrudController : Controller
    {
        public CrudController(IDataProviderDto dataProviderDto)
        {
            this.dataServiceDto = dataProviderDto.CreateDataServiceInstance();
        }

        private readonly DataServiceDto dataServiceDto;

        // GET: api/datasource/crud/{entitySetName}?skip=20&top=10
        [Route("{entitySetName}")]
        [HttpGet]
        public ResultSerialResponse Get(string entitySetName, [FromQuery] QueryParams queryParams)
        {
            return this.dataServiceDto.ApiProviderDto.HandleGet(entitySetName, queryParams);
        }

        // GET: api/datasource/crud/single/{entitySetName}?keys=key1:{key1}
        [Route("single/{entitySetName}")]
        [HttpGet]
        public ResultSingleSerialData GetSingle(string entitySetName, [FromQuery] QueryParams queryParams)
        {
            return this.dataServiceDto.ApiProviderDto.HandleGetSingle(entitySetName, queryParams);
        }

        // GET: api/datasource/crud/many/{entitySetName}?keys=key1:1,2,3,4;key2:4,5,6,7
        [Route("many/{entitySetName}")]
        [HttpGet]
        public ResultSerialData GetMany(string entitySetName, [FromQuery] QueryParams queryParams)
        {
            return this.dataServiceDto.ApiProviderDto.HandleGetMany(entitySetName, queryParams);
        }

        // PUT: api/datasource/crud/{entitySetName}?keys=key1:{key1}
        [Route("{entitySetName}")]
        [HttpPut]
        public ResultSingleSerialData Put(string entitySetName, [FromQuery] QueryParams queryParams, [FromBody] JObject jdto)
        {
            var dto = jdto.ToObject<Dto>();
            return this.dataServiceDto.ApiProviderDto.HandleUpdateEntity(entitySetName, queryParams, dto);
        }

        // PATCH: api/datasource/crud/{entitySetName}?keys=key1:{key1}
        [Route("{entitySetName}")]
        [HttpPatch]
        public ResultSingleSerialData Patch(string entitySetName, [FromQuery] QueryParams queryParams, [FromBody] JObject jdto)
        {
            var dto = jdto.ToObject<Dto>();
            return this.dataServiceDto.ApiProviderDto.HandleUpdateEntity(entitySetName, queryParams, dto);
        }

        // POST: api/datasource/crud/{entitySetName}
        [Route("{entitySetName}")]
        [HttpPost]
        public ResultSingleSerialData Post(string entitySetName, [FromBody] JObject jdto)
        {
            var dto = jdto.ToObject<Dto>();
            return this.dataServiceDto.ApiProviderDto.HandleInsertEntity(entitySetName, dto);
        }

        // DELETE: api/datasource/crud/{entitySetName}?keys=key1:{key1}
        [Route("{entitySetName}")]
        [HttpDelete]
        public ResultSingleSerialData Delete(string entitySetName, [FromQuery] QueryParams queryParams)
        {
            return this.dataServiceDto.ApiProviderDto.HandleDeleteEntity(entitySetName, queryParams);
        }

        // PUT: api/datasource/crud/batch/{entitySetName}
        [Route("batch/{entitySetName}")]
        [HttpPut]
        public List<ResultSingleSerialData> PutBatch(string entitySetName, [FromBody] JObject[] jdtos)
        {
            var dtos = new List<Dto>();
            foreach (var jdto in jdtos)
            {
                dtos.Add(jdto.ToObject<Dto>());
            }
            return this.dataServiceDto.ApiProviderDto.HandleUpdateEntityBatch(entitySetName, dtos.ToArray());
        }

        // PATCH: api/datasource/crud/batch/{entitySetName}
        [Route("{entitySetName}")]
        [HttpPatch]
        public List<ResultSingleSerialData> PatchBatch(string entitySetName, [FromBody] JObject[] jdtos)
        {
            var dtos = new List<Dto>();
            foreach (var jdto in jdtos)
            {
                dtos.Add(jdto.ToObject<Dto>());
            }
            return this.dataServiceDto.ApiProviderDto.HandleUpdateEntityBatch(entitySetName, dtos.ToArray());
        }

        // POST: api/datasource/crud/batch/{entitySetName}
        [Route("{entitySetName}")]
        [HttpPost]
        public List<ResultSingleSerialData> PostBatch(string entitySetName, [FromBody] JObject[] jdtos)
        {
            var dtos = new List<Dto>();
            foreach (var jdto in jdtos)
            {
                dtos.Add(jdto.ToObject<Dto>());
            }
            return this.dataServiceDto.ApiProviderDto.HandleInsertEntityBatch(entitySetName, dtos.ToArray());
        }

        //// DELETE: api/datasource/crud/batch/{entitySetName}
        //[Route("{entitySetName}")]
        //[HttpDelete]
        //public ResultSerialData DeleteBatch1(string entitySetName, [FromBody] JObject[] jdtos)
        //{
        //    var dtos = new List<Dto>();
        //    foreach (var jdto in jdtos)
        //    {
        //        dtos.Add(jdto.ToObject<Dto>());
        //    }
        //    return this.dataServiceDto.ApiProviderDto.HandleDeleteEntityBatch1(entitySetName, dtos.ToArray());
        //}

        // DELETE: api/datasource/crud/batch/{entitySetName}?keys=key1:1,2,3,4;key2:4,5,6,7
        [Route("{entitySetName}")]
        [HttpDelete]
        public ResultSerialData DeleteBatch(string entitySetName, [FromQuery] QueryParams queryParams)
        {
            return this.dataServiceDto.ApiProviderDto.HandleDeleteEntityBatch(entitySetName, queryParams);
        }
    }
}
