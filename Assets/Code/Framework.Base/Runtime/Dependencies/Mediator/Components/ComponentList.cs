using System.Collections.Generic;
using UnityEngine;

namespace Framework.Base.Dependencies.Mediator
{
    internal class ComponentList : BaseComponentList
    {
        #region Methods

        /// <summary>
        /// Adds a list of objects to the end of the ArrayList.
        /// </summary>
        /// <remarks> O(1) | O(N) </remarks>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int Add<T>(T value,in ComponentType type)
        {
            if(value == null)
            {
                Debug.LogWarning("Value is null");
                return default;
            }
            var valueType = typeof(T);
            if (!ComponentsList.ContainsKey(valueType))
            {
                var list = new List<T> {value};
                var index = base.Add(list);
                ComponentsList.Add(valueType, new ComponentData(index, true, type));
                return index;
            }

            if (ComponentsList.TryGetValue(valueType,out ComponentData data) && data.IsExist)
            {
                if (TryGet<T>(out var obj))
                {
                    obj.Add(value);
                    return data.Index;
                }
            }

            Debug.LogWarning($"The object is already registered! {valueType}");
            return -1;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <remarks> O(N) </remarks>
        /// <param name="targetObject"></param>
        /// <returns></returns>
        public bool TryGet<T>(out List<T> targetObject)
        {
            if (ComponentsList.TryGetValue(typeof(T),out ComponentData data) && data.IsExist)
            {
                targetObject = this[typeof(T)] as List<T>;
                if (targetObject != null)
                {
                    switch (data)
                    {
                        case {ComponentType: ComponentType.UnityType}:
                            targetObject.RemoveAll(item => item as UnityEngine.Object == null);
                            break;
                        case {ComponentType: ComponentType.ObjectType}:
                            targetObject.RemoveAll(item => item == null);
                            break;
                    }

                    return true;
                }
            }

            targetObject = default;  // TODO : Mediator - the future needs to be changed to NullObject
            return false;
        }

        public ComponentList(in int index) : base(index) { }

        #endregion
    }
}
