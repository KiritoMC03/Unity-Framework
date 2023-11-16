using System;

namespace GameKit.General.TransactionSystem
{
    public interface ITransactionStrategy
    {
        public Type TransactionType { get; }
        public ITransaction CreateInstance();
    }
}