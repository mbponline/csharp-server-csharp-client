using System;
using System.Collections.Generic;
using System.Linq;

namespace Client.Models.Utils.DAL.Common
{
    public class EntitySet
	{
		public EntitySet(string entityTypeName, Dictionary<string, EntitySet> entitySets, Metadata metadata)
		{
			this.entityTypeName = entityTypeName;
			this.entitySets = entitySets;
			this.metadata = metadata;
			this.key = metadata.EntityTypes[this.entityTypeName].Key;
			this.Items = new List<Entity>();
		}

		private readonly string entityTypeName;
		private Dictionary<string, EntitySet> entitySets;
		private Metadata metadata;

		private readonly string[] key;

		public List<Entity> Items { get; private set; }

		public Entity NavigateSingle(Entity remoteEntity, string[] remoteEntityKey, string[] navigationKey)
		{
			var result = this.Items.FirstOrDefault((it) => this.HaveSameKeysNavigation(it.dto, navigationKey, remoteEntity.dto, remoteEntityKey));
			return result;
		}

		public IEnumerable<Entity> NavigateMulti(Entity remoteEntity, string[] remoteEntityKey, string[] navigationKey)
		{
			var result = this.Items.Where((it) => this.HaveSameKeysNavigation(it.dto, navigationKey, remoteEntity.dto, remoteEntityKey));
			return result;
		}

		public IEnumerable<Entity> NavigateAllRelated(IEnumerable<Dto> remoteDtos, string[] remoteEntityKey, string[] navigationKey)
		{
			var result = this.Items.Where((it) =>
			{
				foreach (var remoteEntity in remoteDtos)
				{
					if (this.HaveSameKeysNavigation(it.dto, navigationKey, remoteEntity, remoteEntityKey))
					{
						return true;
					}
				}
				return false;
			});
			return result;
		}

		public Entity FindByKey(Dto partialDto)
		{
			var entity = this.Items.FirstOrDefault((it) => this.HaveSameKeysLocal(it.dto, partialDto));
			return entity;
		}

		public Entity Find(Func<Entity, bool> predicate)
		{
			var entity = this.Items.FirstOrDefault(predicate);
			return entity;
		}

		public IEnumerable<Entity> Filter(Func<Entity, bool> predicate)
		{
			var entities = this.Items.Where(predicate);
			return entities;
		}

		public void DeleteEntity(Entity entity)
		{
			entity.Detach();
			this.Items.Remove(entity);
		}

		public void DeleteAll()
		{
			foreach (var entity in this.Items)
			{
				entity.Detach();
			}
			this.Items.RemoveAll((it) => true);
		}

		public void Dispose()
		{
			this.DeleteAll();
			this.entitySets = null;
			this.metadata = null;
		}

		public Entity UpdateEntity(Dto dto)
		{
			Entity newItem;
			// se cauta elementul in colectia existenta
			var found = this.FindByKey(dto);
			if (found == null)
			{
				// daca nu a fost gasit se adauga in colectie
				newItem = this.CreateNewItem(dto);
				this.Items.Add(newItem);
			}
			else
			{
				// daca a fost gasit nu se inlocuieste ci se actualizaeza datale
				// astfel ca astfel ca referintele din dataViews existente sa nu se piarda.
				newItem = this.Initialize(dto, found);

			}

			return newItem;
		}

		public void AttachEntitySet(List<Dto> dtos)
		{
			var entities = new List<Entity>();
			foreach (var dto in dtos)
			{
				entities.Add(this.CreateNewItem(dto));
			}
			this.Items = entities;
		}

		private Entity CreateNewItem(Dto dto)
		{
			var entity = new Entity(this.entityTypeName, dto);
			entity.Attach(this.entitySets, this.metadata);
			return entity;
		}

		private Entity Initialize(Dto dto, Entity entity)
		{
			//foreach (var prop in dto)
			//{
			//	entity[prop.Key] = prop.Value;
			//}

			// Nu este nevoie sa se copieze proprietatile.
			// Toate referintele externe se fac la Entity asadar se poate
			// inlocui referinta la Dto fara a afecta integritatea referentiala

			entity.dto = dto;
			return entity;
		}

		private bool HaveSameKeysLocal(Dto localDto, Dto remoteDto)
		{
			for (int i = 0; i < this.key.Length; i++)
			{
				if ((long)localDto[this.key[i]] != (long)remoteDto[this.key[i]])
				{
					return false;
				}
			}
			return true;
		}

		private bool HaveSameKeysNavigation(Dto localDto, string[] keyLocal, Dto remoteDto, string[] keyRemote)
		{
			for (int i = 0; i < keyLocal.Length; i++)
			{
				if ((long)localDto[keyLocal[i]] != (long)remoteDto[keyRemote[i]])
				{
					return false;
				}
			}
			return true;
		}
	}

}