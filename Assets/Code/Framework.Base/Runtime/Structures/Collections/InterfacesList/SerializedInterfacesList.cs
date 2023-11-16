using System;
using System.Collections.Generic;
using General.Extensions;
using UnityEngine;

namespace General
{
    [Serializable]
    public class SerializedInterfacesList<TInterface> : List<Component>, ISerializationCallbackReceiver
        where TInterface : class
    {
        #region Fields

        [SerializeField]
        private List<Component> items;

        #endregion

        #region Constructors

        public SerializedInterfacesList() => items = this;
        public SerializedInterfacesList(int capacity) : base(capacity) => items = this;
        public SerializedInterfacesList(IEnumerable<Component> collection) : base(collection) => items = this;

        #endregion

        #region Methods

        public TInterface GetAt(int index) => items[index] as TInterface;
        public void SetAt(int index, Component item) => items[index] = item;

        public IEnumerable<TInterface> Iterate()
        {
            foreach (Component item in items)
                yield return item as TInterface;
        }

        public bool Contains(TInterface item)
        {
            for (int i = 0; i < items.Count; i++)
                if (item.Equals(items[i] as TInterface))
                    return true;
            return false;
        }

        public bool Remove(TInterface item)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (!item.Equals(items[i] as TInterface)) continue;
                items.RemoveAt(i);
                return true;
            }

            return false;
        }

        public int IndexOf(TInterface item)
        {
            for (int i = 0; i < items.Count; i++)
                if (item.Equals(items[i] as TInterface))
                    return i;
            return -1;
        }

        #endregion

        #region ISerializationCallbackReceiver

        public void OnBeforeSerialize()
        {
            for (int i = 0; i < items.Count; i++)
            {
                Component current = items[i];
                if (current.IsNull() || current is TInterface)
                    continue;
                if (current.TryGetComponent(out TInterface temp))
                {
                    items[i] = temp as Component;
                    return;
                }

                items[i] = default;
            }
        }

        public void OnAfterDeserialize()
        {
        }

        #endregion
    }
}