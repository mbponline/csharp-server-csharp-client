using NavyBlueDtos;
using System.Collections.Generic;
using System.Linq;
using MetadataSrv = NavyBlueDtos.MetadataSrv;

namespace NavyBlueEntities
{

    internal static class DataViewLocalDtoUtils
    {
        public static void FillResultSingleRelatedItems(string entityTypeName, ResultSingleSerialData resultSingleSerialData, string[] expand, DataContext dataContext, MetadataSrv.Metadata metadataSrv)
        {
            if (resultSingleSerialData.Item != null)
            {
                var resultSerialData = new ResultSerialData()
                {
                    Items = new List<Dto>() { resultSingleSerialData.Item },
                    EntityTypeName = entityTypeName,
                    TotalCount = 0,
                    RelatedItems = { }
                };
                FillResultRelatedItems(entityTypeName, resultSerialData, expand, dataContext, metadataSrv);
                resultSingleSerialData.RelatedItems = resultSerialData.RelatedItems;
            }
        }

        public static void FillResultRelatedItems(string entityTypeName, ResultSerialData resultSerialData, string[] expand, DataContext dataContext, MetadataSrv.Metadata metadataSrv)
        {
            if (expand != null && expand.Length > 0 && resultSerialData.Items != null && resultSerialData.Items.Count() > 0)
            {
                var splitExpand = DataUtils.SplitExpand(expand, (el) => el);
                foreach (var branch in DataUtils.NavigationBranch(splitExpand))
                {
                    var navs = DataUtils.BranchToNavigation(entityTypeName, branch, metadataSrv);
                    var lastNav = navs[navs.Count - 1];
                    var rootEntityTypeName = string.Empty;
                    IEnumerable<Dto> rootItems;
                    if (branch.Length == 1)
                    {
                        rootEntityTypeName = entityTypeName;
                        rootItems = resultSerialData.Items;
                    }
                    else
                    {
                        rootEntityTypeName = navs[navs.Count - 2].EntityTypeName;
                        rootItems = resultSerialData.RelatedItems[rootEntityTypeName];
                    }
                    var navigationPropertyName = branch[branch.Length - 1];
                    var entityTypeNameLocal = lastNav.EntityTypeName;
                    var relatedEntityItems = dataContext.GetRelatedEntities(rootEntityTypeName, rootItems, navigationPropertyName).ToList();
                    if (resultSerialData.RelatedItems == null)
                    {
                        resultSerialData.RelatedItems = new Dictionary<string, IEnumerable<Dto>>();
                    }
                    if (!resultSerialData.RelatedItems.ContainsKey(entityTypeNameLocal))
                    {
                        resultSerialData.RelatedItems[entityTypeNameLocal] = relatedEntityItems.Select(it => it.entity.dto);
                    }
                    else
                    {
                        var items = resultSerialData.RelatedItems[entityTypeNameLocal].ToList();
                        PushMultiIfNotThere(relatedEntityItems, items);
                        resultSerialData.RelatedItems[entityTypeNameLocal] = items;
                    }
                }
            }
        }

        public static void PushMultiIfNotThere(IEnumerable<IDerivedEntity> sourceList, List<Dto> destinationList)
        {
            foreach (var item in sourceList)
            {
                if (!destinationList.Contains(item.entity.dto))
                {
                    destinationList.Add(item.entity.dto);
                }
            }
        }
    }

}