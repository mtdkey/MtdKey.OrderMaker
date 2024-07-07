using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace MtdKey.OrderMaker.Core
{
    public static class FuncExtensions
    {

        public static IEnumerable<string> SplitByLength(this string str, int maxLength)
        {
            int index = 0;
            while (true)
            {
                if (index + maxLength >= str.Length)
                {
                    yield return str[index..];
                    yield break;
                }
                yield return str.Substring(index, maxLength);
                index += maxLength;
            }
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool controlCheck, Expression<Func<T, bool>> condition)
        {
            if (controlCheck)
            {
                return query.Where(condition);
            }
            return query;
        }

        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }
    }

}
