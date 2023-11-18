using System;

namespace Framework.Base.Transactions.Test
{
    public class TestStrategy : TransactionStrategy
    {
        private bool useFirst = true;
        
        public override Type TransactionType => typeof(ITestTransaction);
        public bool UseFirst
        {
            get => useFirst;
            set
            {
                useFirst = value;
                LockCurrentTransaction();
            }
        }

        public override ITransaction CreateInstance() => 
            UseFirst ? (ITransaction) new TestTransaction0() : new TestTransaction1();
    }
}