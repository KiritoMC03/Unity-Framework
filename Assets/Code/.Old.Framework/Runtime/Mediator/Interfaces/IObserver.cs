using System;

namespace General.Mediator
{

    public interface IObserver<T> where T : class
    {
        internal ObserverSingleComponent<T> GetBase { get;}
        public event Action<T> ChangeComponent;
        public void Release();
        internal void UpdateComponent(T subject);
    }
    
}