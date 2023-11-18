using System;
using System.Collections.Generic;
using Framework.Idlers.Interfaces;
using Framework.Idlers.Resource;
using UnityEngine;

namespace Framework.Idlers.ResourceTransferHelper
{
    public class ResourcesTransferHelper
    {
        #region Events

        public event Action<IBaseResource> ResourceAcceptStartedCallback;
        public event Action<IBaseResource> ResourceFlyingStartedCallback;
        public event Action<IBaseResource> ResourceAcceptedCallback;
        
        public event Action<IBaseResource, IReceiver<IBaseResource, ResourceType>> ResourcePreSentCallback;
        public event Action<IBaseResource> ResourceSentCallback;

        #endregion
        
        #region Fields

        private AcceptResourcesHelper acceptResourcesHelper;
        private SendResourcesHelper sendResourcesHelper;

        #endregion

        #region Properties

        public AcceptResourcesHelper AcceptResourcesHelper => acceptResourcesHelper;
        public SendResourcesHelper SendResourcesHelper => sendResourcesHelper;

        #endregion

        #region Constructors

        public ResourcesTransferHelper(MonoBehaviour coroutineContainer, 
            PaymentConfig paymentConfig,
            IList<IBaseResource> resourcesList, 
            IList<IBaseResource> fliedResourcesList,
            Transform resourceFlyingTarget, 
            Func<ISender<IBaseResource>, bool> checkCanAcceptDelegate = default,
            Func<IReceiver<IBaseResource, ResourceType>, bool> checkCanSendDelegate = default)
        {
            acceptResourcesHelper = new AcceptResourcesHelper(coroutineContainer, 
                paymentConfig, 
                resourcesList,
                fliedResourcesList, 
                resourceFlyingTarget, 
                checkCanAcceptDelegate);
            
            sendResourcesHelper = new SendResourcesHelper(coroutineContainer,
                paymentConfig,
                resourcesList,
                checkCanSendDelegate);
            
            InitEvents();
        }

        #endregion

        #region Methods

        public void StartResourceAccepting(ISender<IBaseResource> sender, bool doRestart = true) =>
            acceptResourcesHelper.StartTransfer(sender, doRestart);

        public void StopResourceAccepting(ISender<IBaseResource> sender) => 
            acceptResourcesHelper.StopTransfer(sender);

        public void StartResourceSending(IReceiver<IBaseResource, ResourceType> receiver, bool doRestart = true) =>
            sendResourcesHelper.StartTransfer(receiver, doRestart);

        public void StopResourceSending(IReceiver<IBaseResource, ResourceType> receiver) => 
            sendResourcesHelper.StopTransfer(receiver);

        private void InitEvents()
        {
            acceptResourcesHelper.ResourceAcceptStartedCallback += InvokeResourceAcceptStartedCallback;
            acceptResourcesHelper.ResourceFlyingStartedCallback += InvokeResourceFlyingStartedCallback;
            acceptResourcesHelper.ResourceAcceptedCallback += InvokeResourceAcceptedCallback;

            sendResourcesHelper.ResourcePreSentCallback += InvokeResourcePreSentCallback;
            sendResourcesHelper.ResourceSentCallback += InvokeResourceSentCallback;
        }

        public void SetValidateCalculatedResourceTypesDelegate(Func<IEnumerable<ResourceType>, ICollection<ResourceType>> link) =>
            sendResourcesHelper.SetValidateCalculatedResourceTypesDelegate(link);
        public void SetCheckingBeforeResourceSentDelegate(Func<IBaseResource, bool> link) =>
            sendResourcesHelper.SetCheckingBeforeResourceSentDelegate(link);
        private void InvokeResourceAcceptStartedCallback(IBaseResource resource) => ResourceAcceptStartedCallback?.Invoke(resource);
        private void InvokeResourceFlyingStartedCallback(IBaseResource resource) => ResourceFlyingStartedCallback?.Invoke(resource);
        private void InvokeResourceAcceptedCallback(IBaseResource resource) => ResourceAcceptedCallback?.Invoke(resource);
        private void InvokeResourcePreSentCallback(IBaseResource resource,
            IReceiver<IBaseResource, ResourceType> receiver) => ResourcePreSentCallback?.Invoke(resource, receiver);
        private void InvokeResourceSentCallback(IBaseResource resource) => ResourceSentCallback?.Invoke(resource);

        #endregion
    }
}