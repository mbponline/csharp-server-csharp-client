namespace Client.Models.Utils.DAL.Common
{

    public class ServiceLocation<TLocal, TRemote>
    {
        public TLocal Local { get; set; }

        public TRemote Remote { get; set; }
    }

}