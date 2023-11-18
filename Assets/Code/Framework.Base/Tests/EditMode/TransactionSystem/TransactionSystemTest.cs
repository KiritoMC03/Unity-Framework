using Framework.Base.Extensions;
using NUnit.Framework;

namespace Framework.Base.Transactions.Test
{
    public class TransactionSystemTest
    {
        #region Methods

        [Test]
        public void GetTransactionTest()
        {
            ITransactionSystem system = new TransactionSystem();
            TestTransaction0 transaction = system.GetTransaction<TestTransaction0>();
            Assert.NotNull(transaction);
        }

        [Test]
        public void GetTransactionWithStrategyTest()
        {
            bool isValid = true;
            TestStrategy strategy = new TestStrategy();
            ITransactionSystem system = new TransactionSystem(new []{strategy});
            ITestTransaction first = system.GetTransaction<ITestTransaction>();
            strategy.UseFirst = false;
            ITestTransaction second = system.GetTransaction<ITestTransaction>();
            
            Assert.IsTrue(first.NotNull() && second.NotNull() & first is TestTransaction0 & second is TestTransaction1);
        }

        #endregion
    }
}