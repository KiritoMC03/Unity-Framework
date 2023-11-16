using System;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    [Serializable]
    public class ObservableValueLite<T>
    {
        #region Fields

        [SerializeField]
        protected T value;

        [NonSerialized]
        protected List<Action<T>> changedHandlers = new List<Action<T>>(24);

        [NonSerialized]
        protected List<Action<T>> changedOneOffHandlers = new List<Action<T>>(4);

        #endregion

        #region Properties

        public virtual T Value
        {
            get => value;
            set => Change(value);
        }

        #endregion

        #region Methods

        public virtual void OnChanged(Action<T> handler)
        {
            if (handler != null) changedHandlers.Add(handler);
        }

        public virtual void OnChangedOneOff(Action<T> handler)
        {
            if (handler != null) changedOneOffHandlers.Add(handler);
        }

        public void RemoveListener(Action<T> handler) => changedHandlers.Remove(handler);
        public void RemoveOneOffListener(Action<T> handler) => changedOneOffHandlers.Remove(handler);

        protected virtual void Change(T newValue)
        {
            value = newValue;
            for (int i = 0; i < changedHandlers.Count; i++) changedHandlers[i]?.Invoke(newValue);
            for (int i = 0; i < changedOneOffHandlers.Count; i++) changedOneOffHandlers[i]?.Invoke(newValue);
            changedOneOffHandlers.Clear();
        }

        #endregion
    }
}