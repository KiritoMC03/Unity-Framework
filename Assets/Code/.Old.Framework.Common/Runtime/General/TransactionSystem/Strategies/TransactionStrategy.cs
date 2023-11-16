using System;
using General.Mediator;

namespace GameKit.General.TransactionSystem
{
    public abstract class TransactionStrategy : ITransactionStrategy
    {
        #region Fields
        
        private ObserverSingleComponent<ITransactionSystem> transactions;

        #endregion

        #region Constructors

        public TransactionStrategy()
        {
            transactions = new ObserverSingleComponent<ITransactionSystem>(this);
        }

        #endregion
        
        #region Methods

        protected virtual void LockCurrentTransaction() => 
            transactions.Result.RemoveTransaction(TransactionType);

        #endregion
        
        #region ITransactionStrategy
        
        public abstract Type TransactionType { get; }
        public abstract ITransaction CreateInstance();

        #endregion
    }
}