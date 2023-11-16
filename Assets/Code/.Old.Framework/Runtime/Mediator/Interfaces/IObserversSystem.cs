using System;
using System.Collections.Generic;

namespace General.Mediator
{
    internal interface IObserversSystem
    {
        bool Contains(Type type);
        bool Contains<T>(T type);
        bool ContainsValue<T, U>(T type, U value) where T : class where U : ObserverSingleComponent<T>;
        List<U> GetObservers<T, U>(T observerType) where T : class where U : IObserver<T>;
        bool RemoveObserver<T>(ObserverSingleComponent<T> observer) where T : class;
        void AddObserver<T>(ObserverSingleComponent<T> observer) where T : class;
    }
}