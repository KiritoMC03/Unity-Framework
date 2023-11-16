using System;
using UnityEngine;

namespace General.Mediator
{
    internal class SingleComponentList : BaseSingleComponentList
    {
        #region Methods
        

        /// <summary>
        /// Adds an object to the end of the ArrayList.
        /// </summary>
        /// <remarks> O(1) | O(N) </remarks>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int Add<T>(T value, in ComponentType type,IObserversSystem observersSystem ,SetMode setMode = default) where T : class
        {
            var valueType = typeof(T);
            var index = -1;
            if (value == null)
            {
                Debug.LogWarning("Value is null");
                return default;
            }
            if (setMode != default)
            {
                InvokeObservers(value, observersSystem);
                return Add(value, setMode,type);
            }
            if (!ComponentsList.ContainsKey(valueType))
            {
                index = base.Add(value);
                ComponentsList.Add(valueType, new ComponentData(index, true, type));
                InvokeObservers(value, observersSystem);
                return index;
            }
            if (ComponentsList.TryGetValue(valueType, out var data) && !data.IsExist ||
                IsNull(valueType, out var obj))
            {
                InvokeObservers(value, observersSystem);
                return Add(value, data, valueType);
            }

            if (data.ComponentType != type)
            {
                Debug.LogWarning($"The object is already registered another type {valueType} | current type {type} --> exist type {data.ComponentType} >>> USE Fores Mode");
                return -1;
            }

            Debug.LogWarning($"The object is already registered >>> USE Fores Mode");
            return -1;
        }

        public SingleComponentList(in int index) : base(index)
        {
        }

        private int Add(object value, ComponentData data, Type valueType)
        {
            this[data.Index] = value;
            data.IsExist = true;
            ComponentsList[valueType] = data;
            return data.Index;
        }

        #endregion
    }
}