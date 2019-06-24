using NavyBlueDtos;
using System.Collections.Generic;

namespace NavyBlueEntities
{

    public class DataViewRemoteDto
    {
        private readonly string entityTypeName;
        private readonly DataViewDto dataViewDto;

        public DataViewRemoteDto(string entityTypeName, DataViewDto dataViewDto)
        {
            this.entityTypeName = entityTypeName;
            this.dataViewDto = dataViewDto;
        }

        public int Count(QueryObject queryObject)
        {
            return this.dataViewDto.Count(this.entityTypeName, queryObject);
        }

        public ResultSerialData GetItems(QueryObject queryObject)
        {
            return this.dataViewDto.GetItems(this.entityTypeName, queryObject);
        }

        public ResultSingleSerialData GetSingleItem(Dto partialDto, string[] expand = null)
        {
            return this.dataViewDto.GetSingleItem(this.entityTypeName, partialDto, expand);
        }

        public ResultSerialData GetMultipleItems(IEnumerable<Dto> partialDtos, string[] expand = null)
        {
            return this.dataViewDto.GetMultipleItems(this.entityTypeName, partialDtos, expand);
        }

        public ResultSingleSerialData InsertItem(Dto dto)
        {
            return this.dataViewDto.InsertItem(this.entityTypeName, dto);
        }

        public List<ResultSingleSerialData> InsertItems(IEnumerable<Dto> dtos)
        {
            return this.dataViewDto.InsertItems(this.entityTypeName, dtos);
        }

        public ResultSingleSerialData UpdateItem(Dto partialDto)
        {
            return this.dataViewDto.UpdateItem(this.entityTypeName, partialDto);
        }

        public List<ResultSingleSerialData> UpdateItems(IEnumerable<Dto> partialDtos)
        {
            return this.dataViewDto.UpdateItems(this.entityTypeName, partialDtos);
        }

        public ResultSingleSerialData DeleteItem(Dto partialDto)
        {
            return this.dataViewDto.DeleteItem(this.entityTypeName, partialDto);
        }

        public ResultSerialData DeleteItems(IEnumerable<Dto> partialDtos)
        {
            return this.dataViewDto.DeleteItems(this.entityTypeName, partialDtos);
        }
    }

}