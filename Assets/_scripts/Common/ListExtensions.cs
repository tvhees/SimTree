using System;
using System.Linq;
using System.Collections.Generic;

namespace UnityExtensions
{
    public static class ListExtensions
    {
        public static (T, int) Random<T>(this List<T> list)
        {
            if (!list.Any())
            {
                throw new InvalidOperationException("Random<T> used on empty list");
            }

            int i = UnityEngine.Random.Range(0, list.Count);
            return (list[i], i);
        }

        public static (T, int) Random<T>(this T[] array)
        {
            if (!array.Any())
            {
                throw new InvalidOperationException("Random<T> used on empty array");
            }

            int i = UnityEngine.Random.Range(0, array.Length);
            return (array[i], i);
        }
    }
}