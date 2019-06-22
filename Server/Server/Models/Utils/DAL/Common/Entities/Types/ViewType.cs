namespace Server.Models.Utils.DAL.Common
{

    public class ViewType<TLocalEntity, TLocalDto>
    {
        public TLocalEntity EntityView { get; set; }

        public TLocalDto DtoView { get; set; }
    }

}