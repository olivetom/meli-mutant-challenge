using System;
using System.Collections.Generic;
using System.Linq;

namespace meli
{
    static class GroupWhileExtension
    {
        public static IEnumerable<IEnumerable<T>> GroupWhile<T>(this IEnumerable<T> source, Func<T, T, bool> condition)
        {
            T previous = source.FirstOrDefault(); // should be FirstOrDefault
            var list = new List<T>() { previous };
            foreach (T item in source.Skip(1))
            {
                if (condition(previous, item) == false)
                {
                    yield return list;
                    list = new List<T>();
                }
                list.Add(item);
                previous = item;
            }
            yield return list;
        }
    }

  
   

}
