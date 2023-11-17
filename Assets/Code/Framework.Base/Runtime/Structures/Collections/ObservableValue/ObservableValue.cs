using System;
using UnityEngine;

namespace Framework.Base.Collections
{
    [Serializable]
    public class ObservableValue<T>
    {
        #region Events

        public event Action<T> ChangedCallback;

        #endregion

        #region Fields

        [SerializeField]
        private T value;

        #endregion

        #region Properties

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                ChangedCallback?.Invoke(value);
            }
        }

        #endregion

        #region Constructors

        public ObservableValue(T value) => this.value = value;

        #endregion
    }
}