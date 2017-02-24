using System.Collections.Generic;
using System.Linq;

namespace Client.Models.Utils.DAL.Common
{
	public sealed class Entity
	{
		public Entity(string entityTypeName, Dto dto)
		{
			this.entityTypeName = entityTypeName;
			this.dto = dto;
		}

		private Dictionary<string, EntitySet> entitySets;

		private Metadata metadata;

		public string entityTypeName { get; private set; }

		public Dto dto { get; set; }

		public void Attach(Dictionary<string, EntitySet> entitySets, Metadata metadata)
		{
			this.entitySets = entitySets;
			this.metadata = metadata;
		}

		public void Detach()
		{
			this.entitySets = null;
			this.metadata = null;
		}

		public Entity NavigateSingle(string entityTypeName, string navigationPropertyName)
		{
			var navElement = this.metadata.EntityTypes[entityTypeName].NavigationProperties[navigationPropertyName];
			var remoteEntitySet = this.entitySets.ContainsKey(navElement.EntityTypeName) ? this.entitySets[navElement.EntityTypeName] : null;
			return remoteEntitySet != null ? remoteEntitySet.NavigateSingle(this, navElement.KeyLocal, navElement.KeyRemote) : null;
		}

		public IEnumerable<Entity> NavigateMulti(string entityTypeName, string navigationPropertyName)
		{
			var navElement = this.metadata.EntityTypes[entityTypeName].NavigationProperties[navigationPropertyName];
			var remoteEntitySet = this.entitySets.ContainsKey(navElement.EntityTypeName) ? this.entitySets[navElement.EntityTypeName] : null;
			return remoteEntitySet != null ? remoteEntitySet.NavigateMulti(this, navElement.KeyLocal, navElement.KeyRemote) : Enumerable.Empty<Entity>();
		}
	}

}