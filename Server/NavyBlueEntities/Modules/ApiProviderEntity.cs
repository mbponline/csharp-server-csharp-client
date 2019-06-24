using NavyBlueDtos;
using System.Collections.Generic;

namespace NavyBlueEntities
{
    public class ApiProviderEntity
    {
        private readonly ApiProviderDto apiProviderDto;

        public ApiProviderEntity(ApiProviderDto apiProviderDto)
        {
            this.apiProviderDto = apiProviderDto;
        }

        public ResultSerialResponse HandleGet<T>(QueryParams queryParams)
            where T : class
        {
            var entityTypeName = typeof(T).Name;
            var result = this.apiProviderDto.HandleGet(entityTypeName, queryParams);
            return result;
        }

        public ResultSingleSerialData HandleGetSingle<T>(QueryParams queryParams)
            where T : class
        {
            var entityTypeName = typeof(T).Name;
            var resultSingleSerialData = this.apiProviderDto.HandleGetSingle(entityTypeName, queryParams);
            return resultSingleSerialData;
        }

        public ResultSerialData HandleGetMany<T>(QueryParams queryParams)
            where T : class
        {
            var entityTypeName = typeof(T).Name;
            var resultSerialData = this.apiProviderDto.HandleGetMany(entityTypeName, queryParams);
            return resultSerialData;
        }

        public ResultSingleSerialData HandleInsertEntity<T>(Dto dto)
            where T : class
        {
            var entityTypeName = typeof(T).Name;
            var resultSingleSerialData = this.apiProviderDto.HandleInsertEntity(entityTypeName, dto);
            return resultSingleSerialData;
        }

        public List<ResultSingleSerialData> HandleInsertEntityBatch<T>(IEnumerable<Dto> dtos)
            where T : class
        {
            var entityTypeName = typeof(T).Name;
            var resultSingleSerialDataList = this.apiProviderDto.HandleInsertEntityBatch(entityTypeName, dtos);
            return resultSingleSerialDataList;
        }

        public ResultSingleSerialData HandleUpdateEntity<T>(QueryParams queryParams, Dto dto)
            where T : class
        {
            var entityTypeName = typeof(T).Name;
            var resultSingleSerialData = this.apiProviderDto.HandleUpdateEntity(entityTypeName, queryParams, dto);
            return resultSingleSerialData;
        }

        public List<ResultSingleSerialData> HandleUpdateEntityBatch<T>(IEnumerable<Dto> dtos)
            where T : class
        {
            var entityTypeName = typeof(T).Name;
            var resultSingleSerialDataList = this.apiProviderDto.HandleUpdateEntityBatch(entityTypeName, dtos);
            return resultSingleSerialDataList;
        }

        public ResultSingleSerialData HandleDeleteEntity<T>(QueryParams queryParams)
            where T : class
        {
            var entityTypeName = typeof(T).Name;
            var resultSingleSerialData = this.apiProviderDto.HandleDeleteEntity(entityTypeName, queryParams);
            return resultSingleSerialData;
        }

        //// in aceasta varianta informatiile de stergere sunt trimise in body ca dtos[]
        //public ResultSerialData<T> HandleDeleteEntityBatch1<T>(IEnumerable<Dto> dtos)
        //    where T : class
        //{
        //    var entityTypeName = typeof(T).Name;
        //    var resultSerialData = this.apiProviderDto.HandleDeleteEntityBatch1(entityTypeName, dtos);
        //    return resultSerialData;
        //}

        // in aceasta varianta informatiile de stergere sunt trimise in query string
        // similar cu handleGetMany
        public ResultSerialData HandleDeleteEntityBatch<T>(QueryParams queryParams)
            where T : class
        {
            var entityTypeName = typeof(T).Name;
            var resultSerialData = this.apiProviderDto.HandleDeleteEntityBatch(entityTypeName, queryParams);
            return resultSerialData;
        }

    }

}