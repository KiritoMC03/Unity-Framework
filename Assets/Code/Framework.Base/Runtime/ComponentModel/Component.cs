using System;
using Framework.Base.Dependencies.Mediator;
using Framework.Base.Transactions;
using UnityEngine;

namespace Framework.Base.ComponentModel
{
    [Serializable]
    public class Component
    {
        [SerializeField] [HideInInspector]
        protected internal MonoBehaviour source;

        protected ObserverSingleComponent<ITransactionSystem> transactions;

        protected T GetTransaction<T>() where T : ITransaction => transactions.Result.GetTransaction<T>();
        protected internal virtual void Construct() { }
    }
}