using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Base.Extensions;
using Framework.Idlers.Interfaces;
using Framework.Idlers.Resource;
using UnityEngine;

namespace Framework.Idlers.ResourceTransferHelper
{
    public class SendResourcesHelper : BaseTransferHelper<IReceiver<IBaseResource, ResourceType>>
    {
        #region Events

        public event Action<IBaseResource, IReceiver<IBaseResource, ResourceType>> ResourcePreSentCallback;
        public event Action<IBaseResource> ResourceSentCallback;
        public event Func<IBaseResource, bool> CheckingBeforeResourceSentDelegate;
        public event Func<IEnumerable<ResourceType>, ICollection<ResourceType>> ValidateCalculatedResourceTypesDelegate;

        #endregion
        
        #region Fields
        
        protected FindResourceStrategies findResourceStrategies;
        private bool isValidateCalculatedResourceTypesDelegateSetup;
        private bool isCheckingBeforeResourceSentDelegateSetup;

        #endregion

        #region Properties

        public FindResourceStrategyType FindResourceStrategyType { get; set; } = FindResourceStrategyType.Forward;

        #endregion
        
        #region Constructors

        public SendResourcesHelper(MonoBehaviour coroutineContainer, 
            PaymentConfig paymentConfig,
            IList<IBaseResource> resourcesList, 
            Func<IReceiver<IBaseResource, ResourceType>, bool> checkCanExecuteDelegate = default)
        {
            findResourceStrategies = new FindResourceStrategies(resourcesList);
            SetCheckCanExecuteDelegate(checkCanExecuteDelegate);
            RequiredInit(coroutineContainer, paymentConfig, resourcesList);
        }

        #endregion

        #region Methods

        public void SetValidateCalculatedResourceTypesDelegate(Func<IEnumerable<ResourceType>, ICollection<ResourceType>> link)
        {
            ValidateCalculatedResourceTypesDelegate = link;
            isValidateCalculatedResourceTypesDelegateSetup = link.NotNull();
        }

        public void ClearValidateCalculatedResourceTypesDelegate()
        {
            ValidateCalculatedResourceTypesDelegate = default;
            isValidateCalculatedResourceTypesDelegateSetup = false;
        }

        public void SetCheckingBeforeResourceSentDelegate(Func<IBaseResource, bool> link)
        {
            CheckingBeforeResourceSentDelegate = link;
            isCheckingBeforeResourceSentDelegateSetup = link.NotNull();
        }

        public void ClearCheckingBeforeResourceSentDelegate()
        {
            CheckingBeforeResourceSentDelegate = default;
            isCheckingBeforeResourceSentDelegateSetup = false;
        }
        
        private bool TryInvokeSend(IReceiver<IBaseResource, ResourceType> receiver)
        {
            if (isCheckDelegateInited && (bool) checkCanExecuteDelegate?.Invoke(receiver) == false)
                return false;
            
            var matchers = receiver.GetAcceptedMatchers();
            if (!(matchers is ICollection<ResourceType> collection))
                collection = new List<ResourceType>(matchers);
            if (isValidateCalculatedResourceTypesDelegateSetup) 
                collection = ValidateCalculatedResourceTypesDelegate?.Invoke(collection);
            var findResult = findResourceStrategies.TryFindIndex(FindResourceStrategyType, collection, out int index);
            if (!findResult) return false;
            
            return TryPopResourceByIndex(index, receiver);
        }
        
        private bool TryPopResourceByIndex(int index, IReceiver<IBaseResource, ResourceType> receiver)
        {
            if (index < 0)
            {
                Debug.LogWarning("Index can not be less than zero.");
                return false;
            }

            if (resourcesList.Count - 1 < index)
            {
                Debug.LogWarning("The index must not be greater than last list index.");
                return false;
            }
            
            var resource = resourcesList[index];
            if (isCheckingBeforeResourceSentDelegateSetup && 
                !(bool) CheckingBeforeResourceSentDelegate?.Invoke(resource)) return false;
            ResourcePreSentCallback?.Invoke(resource, receiver);
            resourcesList.RemoveAt(index);
            var result = receiver.TryReceive(resource);
            ResourceSentCallback?.Invoke(resource);
            return result;
        }

        #endregion
        
        #region Coroutines

        protected override IEnumerator WorkingRoutine(IReceiver<IBaseResource, ResourceType> receiver)
        {
            var currentPaymentNumber = 0;
            while (true)
            {
                if (receiver.IsNull()) yield break;
                if (receiver.CanReceive)
                {
                    TryInvokeSend(receiver);
                    if (UseIncrementalTransfer) currentPaymentNumber++;
                }

                yield return new WaitForSeconds(CalculateCooldown(currentPaymentNumber));
            }
        }

        #endregion
    }
}