using System.Collections.Generic;
using System.Linq;

namespace NavyBlueDtos
{
    public class ApiProviderDto
    {
        private readonly DataViewDto dataViewDto;
        private readonly ResultSerialUtils resultSerialUtils;
        private readonly MetadataSrv.Metadata metadataSrv;

        public ApiProviderDto(DataViewDto dataViewDto, ResultSerialUtils resultSerialUtils, MetadataSrv.Metadata metadataSrv)
        {
            this.dataViewDto = dataViewDto;
            this.resultSerialUtils = resultSerialUtils;
            this.metadataSrv = metadataSrv;
        }

        public ResultSerialResponse HandleGet(string entitySetName, QueryParams queryParams)
        {
            entitySetName = ApiProviderDtoUtils.FixEntitySetNameCase(entitySetName, this.metadataSrv);
            var queryObject = QueryUtils.RenderQueryObject(queryParams);
            var entityTypeName = ApiProviderDtoUtils.GetEntityTypeName(entitySetName, this.metadataSrv);
            var resultSerialResponse = this.resultSerialUtils.FetchResponseData(entityTypeName, queryObject);
            return resultSerialResponse;
        }

        public ResultSingleSerialData HandleGetSingle(string entitySetName, QueryParams queryParams)
        {
            entitySetName = ApiProviderDtoUtils.FixEntitySetNameCase(entitySetName, this.metadataSrv);
            var queryObject = QueryUtils.RenderQueryObject(queryParams);
            var entityTypeName = ApiProviderDtoUtils.GetEntityTypeName(entitySetName, this.metadataSrv);
            if (queryObject.Keys == null || queryObject.Keys.Count() != 1 || !ApiProviderDtoUtils.ValidKeys(entityTypeName, queryObject.Keys, this.metadataSrv))
            {
                throw new DtosException(code: 400, message: "Bad Request");
            }
            var resultSingleSerialData = this.dataViewDto.GetSingleItem(entityTypeName, queryObject.Keys.FirstOrDefault(), queryObject.Expand);
            return resultSingleSerialData;
        }

        public ResultSerialData HandleGetMany(string entitySetName, QueryParams queryParams)
        {
            entitySetName = ApiProviderDtoUtils.FixEntitySetNameCase(entitySetName, this.metadataSrv);
            var queryObject = QueryUtils.RenderQueryObject(queryParams);
            var entityTypeName = ApiProviderDtoUtils.GetEntityTypeName(entitySetName, this.metadataSrv);
            if (queryObject.Keys == null || queryObject.Keys.Count() == 0 || !ApiProviderDtoUtils.ValidKeys(entityTypeName, queryObject.Keys, this.metadataSrv))
            {
                throw new DtosException(code: 400, message: "Bad Request");
            }
            var resultSerialData = this.dataViewDto.GetMultipleItems(entityTypeName, queryObject.Keys, queryObject.Expand);
            return resultSerialData;
        }

        public ResultSingleSerialData HandleInsertEntity(string entitySetName, Dto dto)
        {
            entitySetName = ApiProviderDtoUtils.FixEntitySetNameCase(entitySetName, this.metadataSrv);
            var entityTypeName = ApiProviderDtoUtils.GetEntityTypeName(entitySetName, this.metadataSrv);
            if (!ApiProviderDtoUtils.ValidDtoKey(entityTypeName, dto, this.metadataSrv))
            {
                throw new DtosException(code: 400, message: "Bad Request");
            }
            var resultSingleSerialData = this.dataViewDto.InsertItem(entityTypeName, dto);
            return resultSingleSerialData;
        }

        public List<ResultSingleSerialData> HandleInsertEntityBatch(string entitySetName, IEnumerable<Dto> dtos)
        {
            entitySetName = ApiProviderDtoUtils.FixEntitySetNameCase(entitySetName, this.metadataSrv);
            var entityTypeName = ApiProviderDtoUtils.GetEntityTypeName(entitySetName, this.metadataSrv);
            if (!ApiProviderDtoUtils.ValidKeys(entityTypeName, dtos, this.metadataSrv))
            {
                throw new DtosException(code: 400, message: "Bad Request");
            }
            var resultSingleSerialDataList = this.dataViewDto.InsertItems(entityTypeName, dtos);
            return resultSingleSerialDataList;
        }

        public ResultSingleSerialData HandleUpdateEntity(string entitySetName, QueryParams queryParams, Dto dto)
        {
            entitySetName = ApiProviderDtoUtils.FixEntitySetNameCase(entitySetName, this.metadataSrv);
            var queryObject = QueryUtils.RenderQueryObject(queryParams);
            var entityTypeName = ApiProviderDtoUtils.GetEntityTypeName(entitySetName, this.metadataSrv);
            if (queryObject.Keys == null || queryObject.Keys.Count() != 1 || !ApiProviderDtoUtils.ValidKeys(entityTypeName, queryObject.Keys, this.metadataSrv))
            {
                throw new DtosException(code: 400, message: "Bad Request");
            }
            if (!ApiProviderDtoUtils.ValidDto(entityTypeName, queryObject.Keys.FirstOrDefault(), dto, this.metadataSrv))
            {
                throw new DtosException(code: 400, message: "Bad Request");
            }
            var resultSingleSerialData = this.dataViewDto.UpdateItem(entityTypeName, DalUtils.Extend(dto, queryObject.Keys.FirstOrDefault()));
            return resultSingleSerialData;
        }

        public List<ResultSingleSerialData> HandleUpdateEntityBatch(string entitySetName, IEnumerable<Dto> dtos)
        {
            entitySetName = ApiProviderDtoUtils.FixEntitySetNameCase(entitySetName, this.metadataSrv);
            var entityTypeName = ApiProviderDtoUtils.GetEntityTypeName(entitySetName, this.metadataSrv);
            if (!ApiProviderDtoUtils.ValidKeys(entityTypeName, dtos, this.metadataSrv))
            {
                throw new DtosException(code: 400, message: "Bad Request");
            }
            var resultSingleSerialDataList = this.dataViewDto.UpdateItems(entityTypeName, dtos);
            return resultSingleSerialDataList;
        }

        public ResultSingleSerialData HandleDeleteEntity(string entitySetName, QueryParams queryParams)
        {
            entitySetName = ApiProviderDtoUtils.FixEntitySetNameCase(entitySetName, this.metadataSrv);
            var queryObject = QueryUtils.RenderQueryObject(queryParams);
            var entityTypeName = ApiProviderDtoUtils.GetEntityTypeName(entitySetName, this.metadataSrv);
            if (queryObject.Keys == null || queryObject.Keys.Count() != 1 || !ApiProviderDtoUtils.ValidDtoKey(entityTypeName, queryObject.Keys.FirstOrDefault(), this.metadataSrv))
            {
                throw new DtosException(code: 400, message: "Bad Request");
            }
            var resultSingleSerialData = this.dataViewDto.DeleteItem(entityTypeName, queryObject.Keys.FirstOrDefault());
            return resultSingleSerialData;
        }

        //// in aceasta varianta informatiile de stergere sunt trimise in body ca dtos[]
        //public ResultSerialData HandleDeleteEntityBatch1(string entitySetName, Dto[] dtos)
        //{
        //    entitySetName = ApiProviderUtils.FixEntitySetNameCase(entitySetName, this.metadataSrv);
        //    var entityTypeName = ApiProviderUtils.GetEntityTypeName(entitySetName, this.metadataSrv);
        //    if (!ApiProviderUtils.ValidKeys(entityTypeName, dtos, this.metadataSrv))
        //    {
        //        throw new DtosException(code: 400, message: "Bad Request");
        //    }
        //    var resultSerialData = this.dataViewDto.DeleteItems(entityTypeName, dtos);
        //    return resultSerialData;
        //}

        // in aceasta varianta informatiile de stergere sunt trimise in query string
        // similar cu handleGetMany
        public ResultSerialData HandleDeleteEntityBatch(string entitySetName, QueryParams queryParams)
        {
            entitySetName = ApiProviderDtoUtils.FixEntitySetNameCase(entitySetName, this.metadataSrv);
            var queryObject = QueryUtils.RenderQueryObject(queryParams);
            var entityTypeName = ApiProviderDtoUtils.GetEntityTypeName(entitySetName, this.metadataSrv);
            if (queryObject.Keys == null || queryObject.Keys.Count() == 0 || !ApiProviderDtoUtils.ValidKeys(entityTypeName, queryObject.Keys, this.metadataSrv))
            {
                throw new DtosException(code: 400, message: "Bad Request");
            }
            var resultSerialData = this.dataViewDto.DeleteItems(entityTypeName, queryObject.Keys);
            return resultSerialData;
        }

    }

}