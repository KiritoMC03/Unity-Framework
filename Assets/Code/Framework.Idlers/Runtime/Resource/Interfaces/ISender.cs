using System;

namespace Framework.Idlers.Interfaces
{
    public interface ISender<T>
    {
        #region Events

        public event Action SentCallback;
        public event Action<T> ItemSentCallback;

        #endregion
        
        #region Properties

        public bool HasObject { get; }

        #endregion
        
        #region Methods

        public bool TryPop(out T target);

        #endregion
    }
}