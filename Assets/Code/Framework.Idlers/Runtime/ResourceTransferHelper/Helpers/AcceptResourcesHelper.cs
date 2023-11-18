using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Framework.Base.Extensions;
using Framework.Idlers.Extensions;
using Framework.Idlers.Interfaces;
using Framework.Idlers.Resource;

namespace Framework.Idlers.ResourceTransferHelper
{
    public class AcceptResourcesHelper : BaseTransferHelper<ISender<IBaseResource>>
    {
        #region Events

        public event Action<IBaseResource> ResourceAcceptStartedCallback;
        public event Action<IBaseResource> ResourceFlyingStartedCallback;
        public event Action<IBaseResource> ResourceAcceptedCallback;

        #endregion
        
        #region Fields
        
        protected IList<IBaseResource> fliedResourcesList;
        protected Transform resourceFlyingTarget;

        #endregion
        
        #region Constructors

        public AcceptResourcesHelper(MonoBehaviour coroutineContainer, 
            PaymentConfig paymentConfig,
            IList<IBaseResource> resourcesList, 
            IList<IBaseResource> fliedResourcesList,
            Transform resourceFlyingTarget, 
            Func<ISender<IBaseResource>, bool> checkCanExecuteDelegate = default)
        {
            this.fliedResourcesList = fliedResourcesList;
            this.resourceFlyingTarget = resourceFlyingTarget;
            SetCheckCanExecuteDelegate(checkCanExecuteDelegate);
            RequiredInit(coroutineContainer, paymentConfig, resourcesList);
        }

        #endregion

        #region Methods

        public void AcceptResource(IBaseResource resource)
        {
            ResourceAcceptStartedCallback?.Invoke(resource);
            fliedResourcesList.Add(resource);
            var flyRoutine = resource.DynamicJumpTo(resourceFlyingTarget, paymentConfig.ResourceJumpDuration);
            flyRoutine.SetLink(resource.CacheGameObject);
            ResourceFlyingStartedCallback?.Invoke(resource);
            flyRoutine.onComplete += () =>
            {
                fliedResourcesList.Remove(resource);
                resourcesList.Add(resource);
                ResourceAcceptedCallback?.Invoke(resource);
            };
        }

        private bool TryInvokeAccept(ISender<IBaseResource> sender)
        {
            if (isCheckDelegateInited && (bool) checkCanExecuteDelegate?.Invoke(sender) == false)
                return false;
            if (!sender.TryPop(out IBaseResource resource)) return false;
            AcceptResource(resource);
            return true;
        }

        #endregion

        #region Coroutines

        protected override IEnumerator WorkingRoutine(ISender<IBaseResource> sender)
        {
            var currentPaymentNumber = 0;
            while (true)
            {
                if (sender.IsNull()) yield break;
                if (sender.HasObject && TryInvokeAccept(sender) && UseIncrementalTransfer)
                    if (UseIncrementalTransfer) currentPaymentNumber++;

                yield return new WaitForSeconds(CalculateCooldown(currentPaymentNumber));
            }
        }

        #endregion
    }
}