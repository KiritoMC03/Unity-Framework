using System;

namespace Framework.Base.Transactions
{
    public interface ITransactionStrategy
    {
        public Type TransactionType { get; }
        public ITransaction CreateInstance();
    }
}