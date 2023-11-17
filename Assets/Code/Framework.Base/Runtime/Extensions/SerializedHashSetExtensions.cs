using System.Collections.Generic;
using Framework.Base.Collections;

namespace Framework.Base.Extensions
{
    public static class SerializedHashSetExtensions
    {
        /// <summary>
        /// Adds multiple elements to the SerializedHashSet.
        /// </summary>
        /// <param name="hashSet">Current SerializedHashSet</param>
        /// <param name="items">Target T range</param>
        /// <typeparam name="T">Generic type of SerializedHashSet</typeparam>
        /// <remarks>O(n+m) | O(m) in Unity2021+</remarks>
        public static void AddRange<T>(this SerializedHashSet<T> hashSet, IReadOnlyList<T> items)
        {
            int itemsNumber = items.Count;
#if UNITY_2021_1_OR_NEWER
            hashSet.EnsureCapacity(hashSet.Count + itemsNumber);
#endif
            for (int i = 0; i < itemsNumber; i++)
                hashSet.Add(items[i]);
        }
    }
}