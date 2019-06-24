using NavyBlueDtos;

namespace NavyBlueEntities
{
    public abstract class DataServiceEntity<TLocalEntity, TLocalDto, TRemoteEntity, TRemoteDto>
        where TLocalEntity : LocalEntityViewsBase
        where TLocalDto : LocalDtoViewsBase
        where TRemoteEntity : RemoteEntityViewsBase
        where TRemoteDto : RemoteDtoViewsBase
    {
        public DataContext DataContext { get; private set; }
        public ServiceLocation<TLocalEntity, TLocalDto, TRemoteEntity, TRemoteDto> From { get; set; }
        public ApiProviderEntity ApiProviderEntity { get; private set; }

        //private readonly DataServiceDto dataServiceDto;

        protected DataServiceEntity(DataServiceDto dataServiceDto)
        {
            //this.dataServiceDto = dataServiceDto;
            this.DataContext = new DataContext(dataServiceDto.MetadataSrv);
            this.ApiProviderEntity = new ApiProviderEntity(dataServiceDto.ApiProviderDto);
        }

    }

}