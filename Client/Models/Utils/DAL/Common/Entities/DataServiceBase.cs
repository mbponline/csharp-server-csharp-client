using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Client.Models.Utils.DAL.Common
{
    public abstract class DataServiceBase<TLocal, TRemote, TFunction, TAction>
        where TLocal : PropertyList
        where TRemote : PropertyList
        where TFunction : OperationsProvider
        where TAction : OperationsProvider
    {
        protected DataServiceBase(Metadata metadata, string baseUrl, string serviceUrl)
        {
            this.DataAdapter = new DataAdapter(metadata, baseUrl, serviceUrl);
            this.DataContext = new DataContext(metadata);
        }

        protected DataAdapter DataAdapter { get; private set; }
        protected DataContext DataContext { get; private set; }
        public ServiceLocation<TLocal, TRemote> From { get; set; }
        public ServiceOperation<TFunction, TAction> Operation { get; set; }

        public void ClearDataContext()
        {
            this.DataContext.Clear();
        }

        protected static async Task<Metadata> GetMetadataAsync(string baseUrl, string serviceUrl)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(serviceUrl + "crud/metadata");
                if (response.IsSuccessStatusCode)
                {
                    var metadata = await response.Content.ReadAsAsync<Metadata>();
                    return metadata;
                }
                else
                {
                    throw new Exception("Could not get metadata");
                }
            };
        }

    }

}