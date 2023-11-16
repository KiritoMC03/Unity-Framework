using System.Collections.Generic;
using UnityEngine;
using GameKit.CraftModule.Interfaces;
using GameKit.CraftModule.Resource;

namespace GameKit.CraftModule.ResourceTransferHelper
{
    public class BackpackBase
    {
        protected MonoBehaviour coroutinesContainer;
        protected BackpackPreferences backpackPreferences;
        protected ResourcesTransferHelper transferHelper;

        #region Methods

        /// <summary>
        /// Is required!
        /// </summary>
        public virtual void Init(MonoBehaviour coroutinesContainer, 
            BackpackPreferences backpackPreferences, 
            IList<IBaseResource> resourcesList, 
            IList<IBaseResource> fliedResourcesList)
        {
            this.coroutinesContainer = coroutinesContainer;
            this.backpackPreferences = backpackPreferences;

            transferHelper = new ResourcesTransferHelper(coroutinesContainer,
                backpackPreferences.paymentConfig,
                resourcesList,
                fliedResourcesList,
                backpackPreferences.anchors[0],
                CheckCanAccept,
                CheckCanSend);
        }

        protected virtual bool CheckCanAccept(ISender<IBaseResource> sender) => true;
        protected virtual bool CheckCanSend(IReceiver<IBaseResource, ResourceType> receiver) => true;

        public virtual void StartResourceAccepting(ISender<IBaseResource> sender) => transferHelper.StartResourceAccepting(sender);
        public virtual void StopResourceAccepting(ISender<IBaseResource> sender) => transferHelper.StopResourceAccepting(sender);
        public virtual void StartResourceSending(IReceiver<IBaseResource, ResourceType> receiver) => transferHelper.StartResourceSending(receiver);
        public virtual void StopResourceSending(IReceiver<IBaseResource, ResourceType> receiver) => transferHelper.StopResourceSending(receiver);
        public virtual void StopAllOperations()
        {
            transferHelper.AcceptResourcesHelper.StopAllOperations();
            transferHelper.SendResourcesHelper.StopAllOperations();
        }

        #endregion
    }
}