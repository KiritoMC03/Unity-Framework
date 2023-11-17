using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Base.SaveLoad;
using Framework.Base.Extensions;
using UnityEngine;

namespace Framework.Base.Collections
{
    [Serializable]
    public class SerializedHashSet<T> : ISerializationCallbackReceiver, ISaveLoadCallbackReceiver, ICollection<T>,
        IEnumerable<T>, IEnumerable
    {
        #region Fields

        [SerializeField]
        private T[] items;

        private HashSet<T> hashSet;
        private bool isDeserializeNow;

        #endregion

        #region Constructors

        public SerializedHashSet() => hashSet = new HashSet<T>();
        public SerializedHashSet(IEnumerable<T> collection) => hashSet = new HashSet<T>(collection);
        public SerializedHashSet(IEqualityComparer<T> comparer) => hashSet = new HashSet<T>(comparer);

        public SerializedHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer) =>
            hashSet = new HashSet<T>(collection, comparer);

        #endregion

        #region Methods ISerializationCallbackReceiver

        public void OnBeforeSerialize()
        {
            if (isDeserializeNow) return;

            if (hashSet.IsNull()) return;
            items = new T[hashSet.Count];
            if (hashSet.Count > 0)
                hashSet.CopyTo(items, 0);
        }

        public void OnAfterDeserialize()
        {
            isDeserializeNow = true;

            if (hashSet.IsNull()) hashSet = new HashSet<T>();
            else hashSet.Clear();
            for (int i = 0; i < items.Length; i++)
            {
                T item = items[i];
                if (hashSet.Contains(item))
                    item = default;
                hashSet.Add(item);
            }

            isDeserializeNow = false;
        }

        #endregion

        #region Properties ICollection

        public int Count => hashSet.Count;
        public bool IsReadOnly => false;

        #endregion

        #region Methods ICollection

        public void Add(T item) => hashSet.Add(item);
        public void Clear() => hashSet.Clear();
        public bool Contains(T item) => hashSet.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => hashSet.CopyTo(array, arrayIndex);
        public bool Remove(T item) => hashSet.Remove(item);
        public IEnumerator<T> GetEnumerator() => hashSet.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region Properties HashSet

        public IEqualityComparer<T> Comparer => hashSet.Comparer;

        #endregion

        #region Methods HashSet

#if UNITY_2021_1_OR_NEWER
        public void EnsureCapacity(int capacity) => hashSet.EnsureCapacity(capacity);
#endif
        public bool Overlaps(IEnumerable<T> other) => hashSet.Overlaps(other);
        public void ExceptWith(IEnumerable<T> other) => hashSet.ExceptWith(other);
        public void IntersectWith(IEnumerable<T> other) => hashSet.IntersectWith(other);
        public int RemoveWhere(Predicate<T> match) => hashSet.RemoveWhere(match);
        public bool SetEquals(IEnumerable<T> other) => hashSet.SetEquals(other);
        public void TrimExcess() => hashSet.TrimExcess();
        public void UnionWith(IEnumerable<T> other) => hashSet.UnionWith(other);
        public bool IsSubsetOf(IEnumerable<T> other) => hashSet.IsSubsetOf(other);
        public bool IsSupersetOf(IEnumerable<T> other) => hashSet.IsSupersetOf(other);
        public void SymmetricExceptWith(IEnumerable<T> other) => hashSet.SymmetricExceptWith(other);
        public bool IsProperSubsetOf(IEnumerable<T> other) => hashSet.IsProperSubsetOf(other);
        public bool IsProperSupersetOf(IEnumerable<T> other) => hashSet.IsProperSupersetOf(other);

        #endregion
    }
}