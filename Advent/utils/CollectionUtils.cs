using System;
using System.Collections.Generic;

namespace Advent.Utils
{
    static class CollectionUtils
    {
        public static void AddOrReplace<TK, TV>(
            this Dictionary<TK, TV> dictionary,
            TK key,
            TV value
        )
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
            }

            dictionary.Add(key, value);
        }

        public static void Increment<TK>(
            this Dictionary<TK, int> dictionary,
            TK key
        )
        {
            if (dictionary.TryGetValue(key, out int value))
            {
                dictionary.Remove(key);
            }
            else
            {
                value = 0;
            }

            dictionary.Add(key, value + 1);
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> callback)
        {
            foreach (T element in collection)
            {
                callback(element);
            }
        }
    }
}
