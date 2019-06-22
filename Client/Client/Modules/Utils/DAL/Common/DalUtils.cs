using System;
using System.Collections;
using System.Collections.Generic;

namespace Client.Modules.Utils.DAL.Common
{

    public static class DalUtils
    {
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
