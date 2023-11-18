using System;

namespace Framework.Base.Dependencies.Mediator
{
    public class ObserverSingleComponent<T> : IObserver<T> where T : class
    {
        private WeakReference<T> weakReference;
        private object appealType;

        public ObserverSingleComponent(object appealType)
        {
            this.appealType = appealType;
            MC.InstanceObserver.RegistrationObserver(this);
        }

        ObserverSingleComponent<T> IObserver<T>.GetBase => this;

        public T Result
        {
            get
            {
                if (weakReference is null)
                {
                    MC.Instance.GetSingleComponent(appealType, out T t);
                    weakReference ??= new WeakReference<T>(t);
                }
                weakReference.TryGetTarget(out T result);
                return result;
            }
        }

        public void Release() => MC.InstanceObserver.RemoveRegistrationObserver(this);

        public event Action<T> ChangeComponent;

        void IObserver<T>.UpdateComponent(T subject)
        {
            weakReference ??= new WeakReference<T>(subject); 
            weakReference.SetTarget(subject);
            ChangeComponent?.Invoke(subject);
        }
    }
}