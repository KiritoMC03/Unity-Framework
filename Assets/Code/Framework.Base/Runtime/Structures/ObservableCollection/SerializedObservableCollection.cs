using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using General.Extensions;
using UnityEngine;

namespace General
{
    [Serializable]
    public class SerializedObservableCollection<T> : ObservableCollection<T>, ISerializationCallbackReceiver
    {
        #region Fields

        [SerializeField]
        private T[] values = new T[0];

        private bool isDeserializeNow;

        #endregion

        #region Constructors

        public SerializedObservableCollection() : base()
        {
        }

        public SerializedObservableCollection(IEnumerable<T> collection) : base(collection) => BakeValues();
        public SerializedObservableCollection(List<T> list) : base(list) => BakeValues();

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
            values.DoWithEveryone(Add);
            isDeserializeNow = false;
        }

        #endregion

        #region Methods

        private void BakeValues()
        {
            values = this.ToArray();
        }

        #endregion
    }
}