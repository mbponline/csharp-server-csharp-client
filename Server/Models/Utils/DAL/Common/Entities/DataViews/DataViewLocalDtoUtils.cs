using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Models.Utils.DAL.Common
{

    internal static class DataViewLocalDtoUtils
    {
        public static void FillResultSingleRelatedItems(string entityTypeName, ResultSingleSerialData resultSingleSerialData, string[] expand, DataContext dataContext, Metadata metadata)
        {
            if (resultSingleSerialData.Item != null)
            {
                var resultSerialData = new ResultSerialData()
                {
                    Items = new List<object>() { resultSingleSerialData.Item },
                    EntityTypeName = entityTypeName,
                    TotalCount = 0,
                    RelatedItems = { }
                };
                FillResultRelatedItems(entityTypeName, resultSerialData, expand, dataContext, metadata);
                resultSingleSerialData.RelatedItems = resultSerialData.RelatedItems;
            }
        }

        public static void FillResultRelatedItems(string entityTypeName, ResultSerialData resultSerialData, string[] expand, DataContext dataContext, Metadata metadata)
        {
            if (expand != null && expand.Length > 0 && resultSerialData.Items != null && resultSerialData.Items.Count() > 0)
            {
                var splitExpand = DataUtils.SplitExpand(expand, (el) => el);
                DataUtils.ForEachNavigation(splitExpand, (branch) =>
                {
                    var navs = DataUtils.BranchToNavigation(entityTypeName, branch, metadata);
                    var lastNav = navs[navs.Count - 1];
                    var rootEntityTypeName = string.Empty;
                    IEnumerable<object> rootItems;
                    if (branch.Count == 1)
                    {
                        rootEntityTypeName = entityTypeName;
                        rootItems = resultSerialData.Items;
                    }
                    else
                    {
                        rootEntityTypeName = navs[navs.Count - 2].EntityTypeName;
                        rootItems = resultSerialData.RelatedItems[rootEntityTypeName];
                    }
                    var navigationPropertyName = branch[branch.Count - 1];
                    var entityTypeNameLocal = lastNav.EntityTypeName;
                    var relatedEntityItems = dataContext.GetRelatedEntities(rootEntityTypeName, rootItems, navigationPropertyName).ToList();
                    if (resultSerialData.RelatedItems == null)
                    {
                        resultSerialData.RelatedItems = new Dictionary<string, IEnumerable<object>>();
                    }
                    if (!resultSerialData.RelatedItems.ContainsKey(entityTypeNameLocal))
                    {
                        resultSerialData.RelatedItems[entityTypeNameLocal] = relatedEntityItems;
                    }
                    else
                    {
                        var items = resultSerialData.RelatedItems[entityTypeNameLocal].ToList();
                        PushMultiIfNotThere(relatedEntityItems, items);
                        resultSerialData.RelatedItems[entityTypeNameLocal] = items;
                    }
                });
            }
        }

        public static void PushMultiIfNotThere(IEnumerable<object> sourceList, List<object> destinationList)
        {
            foreach (var item in sourceList)
            {
                if (!destinationList.Contains(item))
                {
                    destinationList.Add(item);
                }
            }
        }
    }

}