using System;
using Framework.Base.Dependencies.Mediator;

namespace Framework.Base.Transactions
{
    public interface ITransactionSystem : ISingleComponent
    {
        #region Properties

        public bool IsLoggerActive { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns an instance of a transaction. Returns an instance of a transaction. Optionally creates it using the default constructor or strategy (for interfaces and abstract classes).
        /// </summary>
        public T GetTransaction<T>() 
            where T : ITransaction;
        
        /// <summary>
        /// Adds a new strategy for instantiating a transaction.
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="setMode">When selected SetMode.Force will replace the current strategy, for the type, if it exists.</param>
        public void AddStrategy(ITransactionStrategy strategy, SetMode setMode = SetMode.None);
        
        /// <summary>
        /// Remove transactions of type T.
        /// </summary>
        /// <param name="type">Transaction type.</param>
        public void RemoveTransaction(Type type);

        #endregion
    }
}