using System;
using System.Collections.Generic;
using System.Linq;

namespace NavyBlueDtos
{
    public static class DataUtils
    {
        //public static void ForEachNavigationFilter(string parentEntityTypeName, List<NavigationInclude> arr, Action<string, MetadataSrv.NavigationProperty, string> process, MetadataSrv.Metadata metadataSrv)
        //{
        //    foreach (var item in arr)
        //    {
        //        var childNavigationProperty = metadataSrv.EntityTypes[parentEntityTypeName].NavigationProperties[item.NavigationProperty];
        //        process(parentEntityTypeName, childNavigationProperty, item.Filter);
        //        if (item.Include.Count > 0)
        //        {
        //            ForEachNavigationFilter(childNavigationProperty.EntityTypeName, item.Include, process, metadataSrv);
        //        }
        //    }
        //}

        public static IEnumerable<NavigationResult> NavigationFilter(string parentEntityTypeName, List<NavigationInclude> arr, MetadataSrv.Metadata metadataSrv)
        {
            foreach (var item in arr)
            {
                var childNavigationProperty = metadataSrv.EntityTypes[parentEntityTypeName].NavigationProperties[item.NavigationProperty];
                yield return new NavigationResult() { EntityTypeName = parentEntityTypeName, NavigationProperty = childNavigationProperty, Filter = item.Filter };
                if (item.Include.Count > 0)
                {
                    foreach (var navigationResult in NavigationFilter(childNavigationProperty.EntityTypeName, item.Include, metadataSrv))
                    {
                        yield return navigationResult;
                    }
                }
            }
        }

        public static List<NavigationInclude> SplitExpand<T>(T[] data, Func<T, string> getExpand, Action<T, NavigationInclude, bool> process = null)
            where T : class
        {
            var result = new List<NavigationInclude>();
            NavigationInclude node;
            foreach (var el in data)
            {
                var navProps = getExpand(el).Split(new char[] { '.' });
                var arr = result;
                var position = 1;
                foreach (var prop in navProps)
                {
                    node = FindOrCreate(prop, arr);
                    arr = node.Include;
                    if (process != null)
                    {
                        process(el, node, position == navProps.Length);
                    }
                    position++;
                }
            }
            return result;

            /*
            // Explicatii structura

            var expand = ["navprop1.navprop2", "navprop1.navprop2.navprop3", "navprop1.navprop4.navprop5", "navprop6.navprop7.navprop8.navprop9"]
            //... va deveni:

            var splitExpand = [
                {
                    navigationProperty: "navprop1",
                    include: [
                        {
                            navigationProperty: "navprop2",
                            include: [
                                {
                                    navigationProperty: "navprop3",
                                    include: []
                                }
                            ]
                        },
                        {
                            navigationProperty: "navprop4",
                            include: [
                                {
                                    navigationProperty: "navprop5",
                                    include: []
                                }
                            ]
                        }

                    ]
                },
                {
                    navigationProperty: "navprop6",
                    include: [
                        {
                            navigationProperty: "navprop7",
                            include: [
                                {
                                    navigationProperty: "navprop8",
                                    include: [
                                        {
                                            navigationProperty: "navprop9",
                                            include: []
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }

            ]

            */
        }

        public static NavigationInclude FindOrCreate(string navigationProperty, List<NavigationInclude> arr)
        {
            var found = arr.FirstOrDefault((it) => it.NavigationProperty == navigationProperty);
            if (found == null)
            {
                found = new NavigationInclude()
                {
                    NavigationProperty = navigationProperty,
                    Include = new List<NavigationInclude>()
                };
                arr.Add(found);
            }
            return found;
        }

        //public static void ForEachNavigation(List<NavigationInclude> arr, Action<List<string>> process, List<string> branch = null)
        //{
        //    foreach (var item in arr)
        //    {
        //        var branchLocal = new List<string>();
        //        if (branch != null)
        //        {
        //            branchLocal.AddRange(branch);
        //        }
        //        branchLocal.Add(item.NavigationProperty);

        //        process(branchLocal);
        //        if (item.Include.Count > 0)
        //        {
        //            ForEachNavigation(item.Include, process, branchLocal);
        //        }
        //    }
        //}

        public static IEnumerable<string[]> NavigationBranch(List<NavigationInclude> splitExpand, string[] branch = null)
        {
            branch = branch ?? (new string[] { });
            foreach (var item in splitExpand)
            {
                var branchLocal = new List<string>(branch)
                {
                    item.NavigationProperty
                }.ToArray();
                yield return branchLocal;
                if (item.Include.Count > 0)
                {
                    foreach (var branchChild in NavigationBranch(item.Include, branchLocal))
                    {
                        yield return branchChild;
                    }
                }
            }
        }

        public static List<MetadataSrv.NavigationProperty> BranchToNavigation(string entityTypeName, string[] branch, MetadataSrv.Metadata metadataSrv)
        {
            var navigationPropertyList = new List<MetadataSrv.NavigationProperty>();
            var entityTypeNameLocal = entityTypeName;
            foreach (var navigationPropertyName in branch)
            {
                var navigationProperties = metadataSrv.EntityTypes[entityTypeNameLocal].NavigationProperties;
                if (navigationProperties.ContainsKey(navigationPropertyName))
                {
                    navigationPropertyList.Add(navigationProperties[navigationPropertyName]);
                }
                else
                {
                    throw new Exception("invalid navigation property");
                }
                entityTypeNameLocal = navigationProperties[navigationPropertyName].EntityTypeName;
            }
            return navigationPropertyList;
        }
    }

}