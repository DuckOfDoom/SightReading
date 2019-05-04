using System;
using System.Collections.Generic;

namespace DuckOfDoom.SightReading
{
    public static class Extensions
    {
        public static TResult Cast<T, TResult>(this T obj) where TResult : class, T
        {
            return obj as TResult;
        }
        
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var i in items)
            {
                action(i);
                yield return i;
            }
        }
    }
}
