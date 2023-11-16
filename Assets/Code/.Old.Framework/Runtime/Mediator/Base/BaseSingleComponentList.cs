using System;

namespace General.Mediator
{
    internal class BaseSingleComponentList : BaseComponentList
    {
            
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <remarks> O(1) </remarks>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool TryGet(Type type, out object obj)
        {
            return !IsNull(type, out obj);
        }
        protected BaseSingleComponentList(in int index) : base(in index) { }
            
        protected bool IsNull(Type type, out object obj)
        {
            obj = null;// TODO : Mediator - the future needs to be changed to NullObject
            if (!ComponentsList.TryGetValue(type, out ComponentData data) || !data.IsExist) return true;
            obj = this[type];
            if (data.ComponentType == ComponentType.UnityType
                && (UnityEngine.Object) obj == null)
            {
                RemoveType(type,ref obj,ref data);
                return true;
            }
            else if (obj == null)
            {
                RemoveType(type,ref obj, ref data);
                return true;
            }
            return false;
        }
    
        protected void InvokeObservers<T>(T value,IObserversSystem observersSystem) where T : class
        {
            if (!observersSystem.Contains(value)) return;
            var list = observersSystem.GetObservers<T,ObserverSingleComponent<T>>(value);
            for (var index = 0; index < list.Count; index++)
            {
                var item = list[index];
                if (item is IObserver<T> observer)
                {
                    observer.UpdateComponent(value);
                    if (!observersSystem.ContainsValue(value, item)) index--;
                }
            }
        }
    }
}

