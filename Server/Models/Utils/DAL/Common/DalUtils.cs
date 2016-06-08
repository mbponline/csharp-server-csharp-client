using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.Models.Utils.DAL.Common
{

    //public delegate bool IPredicate<T>(T item);

    //public delegate bool ICompare<T>(T a, T b);

    //public delegate void IAction<T>(T item);

    public static class DalUtils
    {
        //public static T FindFirst<T>(IEnumerable<T> items, IPredicate<T> predicate)
        //    where T : class
        //{
        //    foreach (var item in items)
        //    {
        //        if (predicate(item))
        //        {
        //            return item;
        //        }
        //    }
        //    return null;
        //}

        //public static List<T> FindAll<T>(IEnumerable<T> items, IPredicate<T> predicate)
        //    where T : class
        //{
        //    var result = new List<T>();
        //    foreach (var item in items)
        //    {
        //        if (predicate(item))
        //        {
        //            result.Add(item);
        //        }
        //    }
        //    return result;
        //}

        //public static List<T> Distinct<T>(IEnumerable<T> items, ICompare<T> compare = null)
        //    where T : class
        //{
        //    if (compare == null)
        //    {
        //        compare = (a, b) => a == b;
        //    }
        //    var resultItems = new List<T>();
        //    foreach (var item in items)
        //    {
        //        //var found = FindFirst(resultItems, (resultItem) => compare(item, resultItem));
        //        var found = resultItems.FirstOrDefault((resultItem) => compare(item, resultItem));
        //        if (found != null)
        //        {
        //            resultItems.Add(item);
        //        }
        //    }
        //    return resultItems;
        //}

        public static List<T> LeftJoin<TLeft, TRight, T>(IEnumerable<TLeft> leftItems, IEnumerable<TRight> rightItems, Func<TLeft, TRight, bool> condition, Func<TLeft, TRight, T> select)
            where T : class
        {
            var result = new List<T>();
            foreach (var leftItem in leftItems)
            {
                foreach (var rightItem in rightItems)
                {
                    if (condition(leftItem, rightItem))
                    {
                        result.Add(select(leftItem, rightItem));
                    }
                }
            }
            return result;
        }

        //// Experiment 1
        //public static T Extend1<T>(T target, params object[] sources)
        //{
        //    foreach (var source in sources)
        //    {
        //        JObject.FromObject(source).ToObject<T>();
        //    }
        //    return target;
        //}

        //// Experiment 2
        //public static Dto Extend2(Dto target, params Dto[] sources)
        //{
        //    foreach (var source in sources)
        //    {
        //        foreach (var prop in source)
        //        {
        //            target[prop.Key] = prop.Value;
        //        }
        //    }
        //    return target;
        //}

        public static Dto Extend(Dto target, Dto source, bool onlyExistingProperties = false)
        {
            foreach (var prop in source)
            {
                if (!(onlyExistingProperties && target.ContainsKey(prop.Key)))
                {
                    target[prop.Key] = prop.Value;
                }
            }
            return target;
        }

        public static IList CreatList(Type entityType)
        {
            // Info credit: http://stackoverflow.com/questions/4661211/c-sharp-instantiate-generic-list-from-reflected-type/4661237#4661237#answer-4661237
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(entityType);
            var result = (IList)Activator.CreateInstance(constructedListType);
            return result;
        }
    }
}
