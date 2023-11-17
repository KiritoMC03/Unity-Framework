using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Base.Collections
{
    [Serializable]
    public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        #region Events

        public event Action<TKey> ItemAddedCallback;
        public event Action<TKey> ItemDeletedCallback;
        public event Action<TKey> ItemChangedCallback;

        #endregion

        #region Fields

        [SerializeField]
        private List<SerializedDictionaryValue<TKey, TValue>> values =
            new List<SerializedDictionaryValue<TKey, TValue>>();

        private bool canSendEvents = true;
        private bool isDeserializeNow;

        #endregion

        #region Methods

        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            if (canSendEvents) ItemAddedCallback?.Invoke(key);
        }

        public new bool TryGetValue(TKey key, out TValue value) => base.TryGetValue(key, out value);

        public new void Clear()
        {
            if (canSendEvents)
                foreach (KeyValuePair<TKey, TValue> t in this)
                    ItemDeletedCallback?.Invoke(t.Key);
            base.Clear();
        }

        public new bool ContainsKey(TKey key) => base.ContainsKey(key);

        public new bool Remove(TKey key)
        {
            bool result = base.Remove(key);
            if (canSendEvents) ItemDeletedCallback?.Invoke(key);
            return result;
        }

        #endregion

        #region Indexers

        public new TValue this[TKey key]
        {
            get => base[key];
            set
            {
                base[key] = value;
                ItemChangedCallback?.Invoke(key);
            }
        }

        #endregion

        #region ISerializationCallbackReceiver

        public void OnBeforeSerialize()
        {
            if (isDeserializeNow) return;

            canSendEvents = false;
            values.Clear();
            values.Capacity = Count;
            foreach (KeyValuePair<TKey, TValue> item in this)
                values.Add(new SerializedDictionaryValue<TKey, TValue>(item.Key, item.Value));
            canSendEvents = true;
        }

        public void OnAfterDeserialize()
        {
            canSendEvents = false;
            isDeserializeNow = true;

            Clear();
            foreach (SerializedDictionaryValue<TKey, TValue> item in values)
                try
                {
                    Add(item.key, item.value);
                }
                catch (ArgumentException argumentException)
                {
                    Add(default!, item.value);
                }

            isDeserializeNow = false;
            canSendEvents = true;
        }

        #endregion
    }
}