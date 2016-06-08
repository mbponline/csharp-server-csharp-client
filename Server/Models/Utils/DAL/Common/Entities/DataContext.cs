using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Server.Models.Utils.DAL.Common
{
    public class DataContext
    {
        public DataContext(Metadata metadata)
        {
            this.metadata = metadata;
            this.entitySets = new Dictionary<string, IEntitySet<IEntity>>();
        }

        private Metadata metadata;

        public Dictionary<string, IEntitySet<IEntity>> entitySets;

        public Dictionary<string, List<IEntity>> GetDataSets()
        {
            var result = new Dictionary<string, List<IEntity>>();
            foreach (var set in this.entitySets)
            {
                result.Add(set.Key, set.Value.Items);
            }
            return result;
        }

        public IEnumerable<IEntity> GetRelatedEntities(string entityTypeName, IEnumerable<object> entities, string navigationPropertyName)
        {
            var navElement = this.metadata.EntityTypes[entityTypeName].NavigationProperties[navigationPropertyName];
            var remoteEntitySet = this.entitySets.ContainsKey(navElement.EntityTypeName) ? this.entitySets[navElement.EntityTypeName] : null;
            return remoteEntitySet != null ? remoteEntitySet.NavigateAllRelated(entities, navElement.KeyLocal, navElement.KeyRemote) : new List<IEntity>();
        }

        public T CreateItemDetached<T>(string entityTypeName)
        {
            //if (!this.entitySets.ContainsKey(entityTypeName))
            //{
            //    this.InitializeDataSet(entityTypeName);
            //}
            var entityType = Type.GetType(this.metadata.Namespace + "." + entityTypeName);
            var entity = Activator.CreateInstance(entityType);
            return (T)entity;
        }

        public void Clear()
        {
            foreach (var set in this.entitySets)
            {
                this.entitySets[set.Key].DeleteAll();
            }
        }

        public void Dispose()
        {
            // se va apela inainte de incetarea utilizarii obiectului
            // pentru a evita aparitia de memory leaks si a usura activitatea GC-ului
            foreach (var set in this.entitySets)
            {
                this.entitySets[set.Key].Dispose();
            }
            this.entitySets = null;
            this.metadata = null;
        }

        public IEnumerable<T> AttachEntities<T>(ResultSerialData resultSerialData)
            where T : class, IEntity
        {
            var entityType = typeof(T);
            var entityList = (List<T>)this.DtosToEntities(entityType, resultSerialData.Items);
            var dataSet = this.TraverseResults<T>(entityList);
            this.AttachRelatedItems(resultSerialData.RelatedItems);
            return dataSet;
        }

        public T AttachSingleEntitiy<T>(ResultSingleSerialData resultSingleSerialData)
            where T : class, IEntity
        {
            var entityType = typeof(T);
            var entityList = (List<T>)this.DtosToEntities(entityType, new List<object>() { resultSingleSerialData.Item });
            var dataSet = this.TraverseResults<T>(entityList);
            this.AttachRelatedItems(resultSingleSerialData.RelatedItems);
            return dataSet.FirstOrDefault();
        }

        private void AttachRelatedItems(Dictionary<string, IEnumerable<object>> relatedItems)
        {
            if (relatedItems != null)
            {
                foreach (var item in relatedItems)
                {
                    var entityType = Type.GetType(this.metadata.Namespace + "." + item.Key);
                    var entityList = (IEnumerable<object>)this.DtosToEntities(entityType, relatedItems[entityType.Name]);
                    this.TraverseResults(entityType, entityList);
                }
            }
        }

        private IList DtosToEntities(Type entityType, IEnumerable<object> dtos)
        {
            var result = DalUtils.CreatList(entityType);
            foreach (var item in dtos)
            {
                var temp = JObject.FromObject(item).ToObject(entityType);
                result.Add(temp);
            }
            return result;
        }

        private IEnumerable<object> TraverseResults(Type entityType, IEnumerable<object> items)
        {
            if (!this.entitySets.ContainsKey(entityType.Name))
            {
                this.InitializeDataSet(entityType);
            }
            var entities = this.ProcessEntitySet(entityType, items);
            return entities;
        }

        private IEnumerable<T> TraverseResults<T>(IList items)
            where T : class, IEntity
        {
            var entityType = typeof(T);
            if (!this.entitySets.ContainsKey(entityType.Name))
            {
                this.InitializeDataSet(entityType);
            }
            var entities = this.ProcessEntitySet<T>((List<T>)items);
            return entities;
        }

        private IEnumerable<object> ProcessEntitySet(Type entityType, IEnumerable<object> items)
        {
            var entities = new List<object>();

            foreach (var item in items)
            {
                var newEntity = this.ProcessEntity(entityType, item);
                entities.Add(newEntity);
            }
            return entities;
        }

        private IEnumerable<T> ProcessEntitySet<T>(List<T> items)
            where T : class, IEntity
        {
            var entities = new List<T>();
            foreach (var item in items)
            {
                var newEntity = this.ProcessEntity<T>(item);
                entities.Add(newEntity);
            }
            return entities;
        }

        private object ProcessEntity(Type entityType, object item)
        {
            var newItem = this.entitySets[entityType.Name].UpdateEntity((IEntity)item);
            return newItem;
        }

        private T ProcessEntity<T>(T item)
            where T : class, IEntity
        {
            var entityType = typeof(T);
            var newItem = this.entitySets[entityType.Name].UpdateEntity(item);
            return newItem as T;
        }

        private void InitializeDataSet(Type entityType)
        {
            // Initializeaza EntitySet-ul precizat la momentul utilizarii (Lazy)
            var entitySet = new EntitySet<IEntity>(entityType, this.entitySets, this.metadata);
            this.entitySets[entityType.Name] = entitySet;
        }
    }
}