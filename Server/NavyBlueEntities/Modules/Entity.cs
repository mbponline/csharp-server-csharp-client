using NavyBlueDtos;
using System.Collections.Generic;
using System.Linq;
using MetadataSrv = NavyBlueDtos.MetadataSrv;

namespace NavyBlueEntities
{
    public sealed class Entity
    {
        private Dictionary<string, IEntitySet<IDerivedEntity>> entitySets;
        private MetadataSrv.Metadata metadataSrv;
        public string entityTypeName { get; private set; }
        public Dto dto { get; set; }

        public Entity(string entityTypeName, Dto dto)
        {
            this.entityTypeName = entityTypeName;
            this.dto = dto;
        }

        public void Attach(Dictionary<string, IEntitySet<IDerivedEntity>> entitySets, MetadataSrv.Metadata metadataSrv)
        {
            this.entitySets = entitySets;
            this.metadataSrv = metadataSrv;
        }

        public void Detach()
        {
            this.entitySets = null;
            this.metadataSrv = null;
        }

        public TResult NavigateSingle<TResult>(string entityTypeName, string navigationPropertyName)
            where TResult : class, IDerivedEntity
        {
            var navElement = this.metadataSrv.EntityTypes[entityTypeName].NavigationProperties[navigationPropertyName];
            var remoteEntitySet = this.entitySets.ContainsKey(navElement.EntityTypeName) ? (EntitySet<TResult>)this.entitySets[navElement.EntityTypeName] : null;
            return remoteEntitySet != null ? remoteEntitySet.NavigateSingle(this, navElement.KeyLocal, navElement.KeyRemote) : null;
        }

        public IEnumerable<TResult> NavigateMulti<TResult>(string entityTypeName, string navigationPropertyName)
            where TResult : class, IDerivedEntity
        {
            var navElement = this.metadataSrv.EntityTypes[entityTypeName].NavigationProperties[navigationPropertyName];
            var remoteEntitySet = this.entitySets.ContainsKey(navElement.EntityTypeName) ? (EntitySet<TResult>)this.entitySets[navElement.EntityTypeName] : null;
            return remoteEntitySet != null ? remoteEntitySet.NavigateMulti(this, navElement.KeyLocal, navElement.KeyRemote) : Enumerable.Empty<TResult>();
        }
    }

}