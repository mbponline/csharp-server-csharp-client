using MetadataCli = Client.Modules.Utils.DAL.Common.MetadataCli;

namespace Client.Modules.Utils.DAL.Common
{
    public abstract class DataServiceBase<TLocal, TRemote, TFunction, TAction>
        where TLocal : PropertyList
        where TRemote : PropertyList
        where TFunction : OperationsProvider
        where TAction : OperationsProvider
    {
        protected DataServiceBase(string baseUrl, string apiUrl, MetadataCli.Metadata metadataCli)
        {
            this.DataAdapter = new DataAdapter(baseUrl, apiUrl, metadataCli);
            this.DataContext = new DataContext("Client.Modules.Utils.DAL", metadataCli);
        }

        protected DataAdapter DataAdapter { get; private set; }
        protected DataContext DataContext { get; private set; }
        public ServiceLocation<TLocal, TRemote> From { get; set; }
        public ServiceOperation<TFunction, TAction> Operation { get; set; }

        public void ClearDataContext()
        {
            this.DataContext.Clear();
        }

    }

}