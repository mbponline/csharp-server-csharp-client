using System.Collections.Generic;
using System.Linq;
using MetadataCli = Client.Modules.Utils.DAL.Common.MetadataCli;

namespace Client.Modules.Utils.DAL.Common
{
    public sealed class Entity
    {
        public Entity(string entityTypeName, Dto dto)
        {
            this.entityTypeName = entityTypeName;
            this.dto = dto;
        }

        private Dictionary<string, IEntitySet<IDerivedEntity>> entitySets;

        private MetadataCli.Metadata metadataCli;

        public string entityTypeName { get; private set; }

        public Dto dto { get; set; }

        public void Attach(Dictionary<string, IEntitySet<IDerivedEntity>> entitySets, MetadataCli.Metadata metadataCli)
        {
            this.entitySets = entitySets;
            this.metadataCli = metadataCli;
        }

        public void Detach()
        {
            this.entitySets = null;
            this.metadataCli = null;
        }

        public TResult NavigateSingle<TResult>(string entityTypeName, string navigationPropertyName)
            where TResult : class, IDerivedEntity
        {
            var navElement = this.metadataCli.EntityTypes[entityTypeName].NavigationProperties[navigationPropertyName];
            var remoteEntitySet = this.entitySets.ContainsKey(navElement.EntityTypeName) ? (EntitySet<TResult>)this.entitySets[navElement.EntityTypeName] : null;
            return remoteEntitySet != null ? remoteEntitySet.NavigateSingle(this, navElement.KeyLocal, navElement.KeyRemote) : null;
        }

        public IEnumerable<TResult> NavigateMulti<TResult>(string entityTypeName, string navigationPropertyName)
            where TResult : class, IDerivedEntity
        {
            var navElement = this.metadataCli.EntityTypes[entityTypeName].NavigationProperties[navigationPropertyName];
            var remoteEntitySet = this.entitySets.ContainsKey(navElement.EntityTypeName) ? (EntitySet<TResult>)this.entitySets[navElement.EntityTypeName] : null;
            return remoteEntitySet != null ? remoteEntitySet.NavigateMulti(this, navElement.KeyLocal, navElement.KeyRemote) : Enumerable.Empty<TResult>();
        }
    }

}