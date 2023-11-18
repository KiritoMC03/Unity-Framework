using System;
using System.Collections.Generic;

namespace Framework.Base.Dependencies.Mediator
{
    internal class ObserversSystem : IObserversSystem
    {
        #region Fields

        private readonly Dictionary<Type, object> observers;

        #endregion


        #region Methods

        public bool Contains(Type type)
        {
            return observers.ContainsKey(type);
        }
        
        public bool Contains<T>(T type)
        {
            return observers.ContainsKey(typeof(T));
        }

        public bool ContainsValue<T, U>(T type, U value) where T : class where U : ObserverSingleComponent<T>
        {
            if (observers.TryGetValue(typeof(T), out object values))
            {
                var list = values as List<ObserverSingleComponent<T>>;
                return list.Contains(value);
            }
            return false;
        }

        public List<U> GetObservers<T, U>(T observerType)where T : class  where U : IObserver<T>
        {
            return observers[typeof(T)] as List<U>;
        }

        public bool RemoveObserver<T>(ObserverSingleComponent<T> observer) where T : class
        {
            if (!observers.ContainsKey(typeof(T))) return false;
            var ob = observers[typeof(T)] as List<ObserverSingleComponent<T>>;
            ob.Remove(observer);
            return true;
        }

        public void AddObserver<T>(ObserverSingleComponent<T> observer) where T : class
        {
            if (observers.ContainsKey(typeof(T)))
            {
                var ob = observers[typeof(T)] as List<ObserverSingleComponent<T>>;
                ob.Add(observer);
            }
            else
            {
                var list = new List<ObserverSingleComponent<T>> {observer};
                observers.Add(typeof(T),list);
            }
        }
        

        public ObserversSystem(int capacity = 20)
        {
            observers = new Dictionary<Type, object>(capacity);
        }

        #endregion
        
    }
}
