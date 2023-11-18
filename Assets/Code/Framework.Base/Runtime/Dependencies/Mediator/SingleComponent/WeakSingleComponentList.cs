using System;
using UnityEngine;

namespace Framework.Base.Dependencies.Mediator
{
    internal class WeakSingleComponentList : BaseSingleComponentList
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
            var weakReference = new WeakReference<T>(value);
            if (value == null)
            {
                Debug.LogWarning("Value is null");
                return default;
            }
            if (!ComponentsList.ContainsKey(valueType))
            {
                index = base.Add(weakReference);
                ComponentsList.Add(valueType, new ComponentData(index, true, type));
                InvokeObservers(value, observersSystem);
                return index;
            }
            if (ComponentsList.TryGetValue(valueType, out var data) && !data.IsExist ||
                IsNull(valueType, out var obj) || 
                ComponentsList.TryGetValue(valueType, out data) && data.IsExist && setMode == SetMode.Force )
            {
                InvokeObservers(value, observersSystem);
                return Add(weakReference,value, data, valueType);
            }

            if (data.ComponentType != type)
            {
                Debug.LogWarning($"The object is already registered another type {valueType} | current type {type} --> exist type {data.ComponentType} >>> USE Fores Mode");
                return -1;
            }

            Debug.LogWarning($"The object is already registered >>> USE Fores Mode");
            return -1;
        }

        public WeakSingleComponentList(in int index) : base(index)
        {
        }

        private int Add<T,U>(T weakValue,U value, ComponentData data, Type valueType) 
            where T : class 
            where U : class
        {
            if (this[data.Index].GetType() == weakValue.GetType())
                (this[data.Index] as WeakReference<U>)?.SetTarget(value);
            else
                this[data.Index] = weakValue;
            data.IsExist = true;
            ComponentsList[valueType] = data;
            return data.Index;
        }

        #endregion
    }
}