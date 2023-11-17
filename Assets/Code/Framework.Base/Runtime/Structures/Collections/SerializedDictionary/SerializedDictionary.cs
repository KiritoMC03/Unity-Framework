using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Base.Collections
{
    [Serializable]
    public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        #region Fields

        [SerializeField]
        private List<SerializedDictionaryValue<TKey, TValue>> values =
            new List<SerializedDictionaryValue<TKey, TValue>>();

        private bool isDeserializeNow;

        #endregion

        #region Constructors

        public SerializedDictionary()
        {
        }

        public SerializedDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) => BakeValues();

        public SerializedDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) :
            base(dictionary, comparer) => BakeValues();

        public SerializedDictionary(int capacity) : base(capacity) => BakeValues();

        public SerializedDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) =>
            BakeValues();

        public SerializedDictionary(IEqualityComparer<TKey> comparer) : base(comparer) => BakeValues();

        #endregion

        #region ISerializationCallbackReceiver

        public void OnBeforeSerialize()
        {
            if (isDeserializeNow) return;
            BakeValues();
        }

        public void OnAfterDeserialize()
        {
            isDeserializeNow = true;

            Clear();

            foreach (SerializedDictionaryValue<TKey, TValue> item in values)
                try
                {
                    Add(item.key, item.value);
                }
                catch (ArgumentException)
                {
                    Add(default, item.value);
                }

            isDeserializeNow = false;
        }

        #endregion

        #region Methods

        private void BakeValues()
        {
            values.Clear();
            values.Capacity = Count;
            foreach (KeyValuePair<TKey, TValue> item in this)
                values.Add(new SerializedDictionaryValue<TKey, TValue>(item.Key, item.Value));
        }

        #endregion
    }
}