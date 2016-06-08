using System.Collections.Generic;
using System.Linq;

namespace Client.Models.Utils.DAL.Common
{
    public abstract class Entity : Dto, IEntity
    {
        protected Entity()
        {
        }

        private Dictionary<string, IEntitySet<IEntity>> dataSets;
        private Metadata metadata;

        public void _attach(Dictionary<string, IEntitySet<IEntity>> dataSets, Metadata metadata)
        {
            this.dataSets = dataSets;
            this.metadata = metadata;
        }

        public void _detach()
        {
            this.dataSets = null;
            this.metadata = null;
        }

        protected TResult NavigateSingle<TResult>(string entityTypeName, string navigationPropertyName)
            where TResult : class, IEntity
        {
            var navElement = this.metadata.EntityTypes[entityTypeName].NavigationProperties[navigationPropertyName];
            var remoteEntitySet = this.dataSets.ContainsKey(navElement.EntityTypeName) ? this.dataSets[navElement.EntityTypeName] : null;
            return remoteEntitySet != null ? remoteEntitySet.NavigateSingle(this, navElement.KeyLocal, navElement.KeyRemote) as TResult : null;
        }

        protected IEnumerable<TResult> NavigateMulti<TResult>(string entityTypeName, string navigationPropertyName)
            where TResult : class, IEntity
        {
            var navElement = this.metadata.EntityTypes[entityTypeName].NavigationProperties[navigationPropertyName];
            var remoteEntitySet = this.dataSets.ContainsKey(navElement.EntityTypeName) ? this.dataSets[navElement.EntityTypeName] : null;
            return remoteEntitySet != null ? remoteEntitySet.NavigateMulti(this, navElement.KeyLocal, navElement.KeyRemote).Select((it) => it as TResult) : new List<TResult>();
        }
    }

}