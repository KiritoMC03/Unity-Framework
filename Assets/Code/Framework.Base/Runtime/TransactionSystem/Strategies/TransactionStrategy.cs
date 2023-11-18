using System;
using Framework.Base.Dependencies.Mediator;

namespace Framework.Base.Transactions
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