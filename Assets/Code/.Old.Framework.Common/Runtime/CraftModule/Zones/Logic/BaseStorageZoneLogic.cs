using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameKit.CraftModule.Zones
{
    public abstract class BaseStorageZoneLogic<TContent>
    {
        #region Events

        public event Action ReplenishedCallback;
        public event Action SentCallback;

        #endregion

        #region Fields
        
        protected bool isInitialized;

        #endregion

        #region Properties

        /// Zero by default
        public virtual int Capacity { get; protected set; }
        /// Zero by default
        public virtual int ObjectsCount { get; } = 0;
        /// Zero by default
        public virtual bool CanReplenish { get; } = false;
        /// Zero by default
        public virtual bool HasObject { get; } = false;
        /// Zero by default
        public virtual List<TContent> ContainedObjects { get; } = default;

        #endregion

        #region Methods

        /// <summary>
        /// Always return false
        /// </summary>
        public virtual bool TryReplenish(TContent target)
        {
            return false;
        }

        /// <summary>
        /// Always return false and set default to result
        /// </summary>
        public virtual bool TryPopObject(out TContent result)
        {
            result = default;
            return false;
        }

        /// <summary>
        /// Always return Vector3.zero
        /// </summary>
        public virtual Vector3 GetItemPosition(int index = -1) => Vector3.zero;

        /// <summary>
        /// Default capacity refreshing
        /// </summary>
        public virtual void RefreshCapacity(int newValue) => Capacity = newValue;

        protected void InvokeReplenishedCallback() => ReplenishedCallback?.Invoke();
        protected void InvokeSentCallback() => SentCallback?.Invoke();

        protected bool CheckInitialized()
        {
            if (!isInitialized) Debug.LogWarning($"Zone not isInitialized!");
            return isInitialized;
        }

        #endregion
    }
}