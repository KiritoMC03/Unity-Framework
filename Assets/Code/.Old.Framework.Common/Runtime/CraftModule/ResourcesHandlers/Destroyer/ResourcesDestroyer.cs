using System;
using System.Collections.Generic;
using GameKit.CraftModule.Resource;
using GameKit.General.Extensions;
using General.Mediator;
using UnityEngine;

namespace GameKit.CraftModule.ResourcesHandlers
{
    public class ResourcesDestroyer : MonoBehaviour, IResourceReceiver
    {
        #region Fields

        [SerializeField]
        protected List<ResourceType> acceptedResourcesTypes;

        [SerializeField]
        protected float resourceJumpDuration = 0.5f;

        [SerializeField]
        protected Vector3 jumpOffset;
        
        protected ResourcesCreator creator;
        protected Transform cacheTransform;

        #endregion

        #region Properties

        public virtual Transform CacheTransform
        {
            get
            {
                if (cacheTransform == null)
                {
                    cacheTransform = transform;
                }

                return cacheTransform;
            }
        }

        #endregion
        
        #region IResourceReceiver

        public event Action ReceivedCallback;
        public event Action<IBaseResource> ItemReceivedCallback;
        public virtual bool CanReceive => true;

        public IEnumerable<ResourceType> GetAcceptedMatchers() => acceptedResourcesTypes;

        public virtual bool TryReceive(IBaseResource target)
        {
            target.JumpTo(CacheTransform.position + jumpOffset, resourceJumpDuration).onComplete += () =>
            {
                creator.DestroyResource(target.CacheGameObject);
                ItemReceivedCallback?.Invoke(target);
                ReceivedCallback?.Invoke();
            };
            return true;
        }

        protected void InvokeReceivedCallback() => ReceivedCallback?.Invoke();
        protected void InvokeItemReceivedCallback(IBaseResource resource) => ItemReceivedCallback?.Invoke(resource);

        #endregion

        #region Methods
        
        public virtual void Init()
        {
            MC.Instance.GetSingleComponent(this, out creator);
        }
        
        #endregion
    }
}