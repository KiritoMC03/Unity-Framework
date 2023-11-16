using System;
using UnityEngine;

namespace General
{
    /// <summary>
    ///  For link MonoBehaviours.
    /// </summary>
    [Serializable]
    public class InterfaceItem<T> where T : class
    {
        [SerializeField]
        private Component component;

        public T Interface => component as T;
        public Component Component => component;

        public void Set(Component component)
        {
            if (!(component is T))
            {
                InvalidCastException exception =
                    new InvalidCastException($"Cannot convert {component.GetType()} to {typeof(T)}");
                Debug.LogException(exception, component);
            }
            else
            {
                this.component = component;
            }
        }
    }
}