using NavyBlueDtos;
using System.Collections.Generic;

namespace NavyBlueEntities
{
    public class RemoteDtoViewsBase : Dictionary<string, DataViewRemoteDto>
    {
        private readonly DataViewDto dataViewDto;

        public RemoteDtoViewsBase(DataViewDto dataViewDto)
        {
            this.dataViewDto = dataViewDto;
        }

        protected DataViewRemoteDto GetPropertyValue(string entityTypeName)
        {
            DataViewRemoteDto instance;
            if (this.ContainsKey(entityTypeName))
            {
                instance = this[entityTypeName];
            }
            else
            {
                instance = new DataViewRemoteDto(entityTypeName, this.dataViewDto);
                this[entityTypeName] = instance;
            }
            return instance;
        }
    }
}
