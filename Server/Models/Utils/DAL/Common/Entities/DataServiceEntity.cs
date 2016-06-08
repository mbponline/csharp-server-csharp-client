namespace Server.Models.Utils.DAL.Common
{
    public abstract class DataServiceEntity<TLocalEntity, TLocalDto, TRemoteEntity, TRemoteDto> : DataServiceDto
        where TLocalEntity : PropertyList
        where TLocalDto : PropertyList
        where TRemoteEntity : PropertyList
        where TRemoteDto : PropertyList
    {
        protected DataServiceEntity(string metadataFileName = "", string connectionString = "")
            : base(metadataFileName, connectionString)
        {
            this.DataContext = new DataContext(this.Metadata);
        }

        public DataContext DataContext { get; private set; }

        public ServiceLocation<TLocalEntity, TLocalDto, TRemoteEntity, TRemoteDto> From { get; set; }
    }

}