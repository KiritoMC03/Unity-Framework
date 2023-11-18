using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Base.Extensions;
using Framework.Idlers.Resource;
using UnityEngine;

namespace Framework.Idlers.ResourceTransferHelper
{
    public abstract class BaseTransferHelper<T>
    {
        #region Fields
        
        protected Dictionary<T, Coroutine> routines = new Dictionary<T, Coroutine>(5);

        protected MonoBehaviour coroutineContainer;
        protected PaymentConfig paymentConfig;
        protected IList<IBaseResource> resourcesList = new List<IBaseResource>(10);
        protected Func<T, bool> checkCanExecuteDelegate;
        protected bool isCheckDelegateInited;

        #endregion

        #region Properties

        public virtual Dictionary<T, Coroutine> Routines => routines;
        public bool UseIncrementalTransfer { get; set; }

        #endregion

        #region Methods

        public void RequiredInit(MonoBehaviour coroutineContainer, PaymentConfig paymentConfig, IList<IBaseResource> resourcesList)
        {
            this.resourcesList = resourcesList;
            this.coroutineContainer = coroutineContainer;
            this.paymentConfig = paymentConfig;
        }

        public virtual void StartTransfer(T source, bool doRestart = true)
        {
            if (routines.ContainsKey(source))
            {
                if (!doRestart) return;
                StopTransfer(source);
            }
            routines.Add(source, coroutineContainer.StartCoroutine(WorkingRoutine(source)));
        }

        public virtual void StopTransfer(T source)
        {
            if (!routines.ContainsKey(source)) return;
            var current = routines[source];
            if (current.NotNull()) coroutineContainer.StopCoroutine(current);
            routines.Remove(source);
        }

        public virtual void StopAllOperations()
        {
            if (coroutineContainer.IsNull() || routines.IsNull()) return;
            foreach (var routine in routines.Values)
                if (routine.NotNull()) coroutineContainer.StopCoroutine(routine);
            routines.Clear();
        }

        public void SetCheckCanExecuteDelegate(Func<T, bool> value)
        {
            checkCanExecuteDelegate = value;
            isCheckDelegateInited = value.NotNull();
        }

        public void ClearCheckCanExecuteDelegate()
        {
            checkCanExecuteDelegate = default;
            isCheckDelegateInited = false;
        }

        protected virtual float CalculateCooldown(int currentPaymentNumber)
        {
            if (!UseIncrementalTransfer) return paymentConfig.DefaultCooldownTime;
            var cooldownTime = paymentConfig.DefaultCooldownTime - (currentPaymentNumber * paymentConfig.StepProgression);
            cooldownTime = Mathf.Clamp(cooldownTime, paymentConfig.MinCooldownTime, paymentConfig.DefaultCooldownTime);
            return cooldownTime;
        }
        
        #endregion

        #region Coroutines

        protected abstract IEnumerator WorkingRoutine(T sender);

        #endregion
    }
}