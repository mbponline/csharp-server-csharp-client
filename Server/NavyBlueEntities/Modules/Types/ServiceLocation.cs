﻿namespace NavyBlueEntities
{

    public class ServiceLocation<TLocalEntity, TLocalDto, TRemoteEntity, TRemoteDto>
    {
        public ViewType<TLocalEntity, TLocalDto> Local { get; set; }

        public ViewType<TRemoteEntity, TRemoteDto> Remote { get; set; }
    }

}