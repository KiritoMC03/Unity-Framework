using System;
using System.Collections.Generic;
using General.Extensions;
using General.Mediator;
using UnityEngine;

namespace GameKit.General.TransactionSystem
{
    public class TransactionSystem : ITransactionSystem
    {
        #region Fields

        protected Dictionary<Type, ITransaction> savedTransactions;

        protected readonly Dictionary<Type, ITransactionStrategy> strategies;
        
        protected static readonly string AddStrategyMethodName = $"{nameof(AddStrategy)}";
        protected const string TypeIsNull = "The type being passed is Null.";
        protected const string StrategyIsNull = "The strategy being passed is Null.";
        protected const int SavedTransactionsCapacity = 24;
        protected const int StrategiesCapacity = 8;

        #endregion

        #region Properties

        public virtual bool IsLoggerActive { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a TransactionSystem instance
        /// </summary>
        /// <param name="startedStrategies">A list containing strategies for creating transactions. If you need real-time add strategies, you must call the AddStrategy() method.</param>
        public TransactionSystem(IReadOnlyList<ITransactionStrategy> startedStrategies = default)
        {
            this.strategies = new Dictionary<Type, ITransactionStrategy>(StrategiesCapacity);
            this.savedTransactions = new Dictionary<Type, ITransaction>(SavedTransactionsCapacity);
            if (startedStrategies.NotNull())
                startedStrategies.DoWithEveryone(s => strategies.Add(s.TransactionType, s));
            MC.Instance.Add<ITransactionSystem>(this, SetMode.Force);
        }

        #endregion

        #region ITransactionSystem
        
        /// <summary>
        /// Returns an instance of a transaction. Returns an instance of a transaction. Optionally creates it using the default constructor or strategy (for interfaces and abstract classes).
        /// </summary>
        public virtual T GetTransaction<T>()
            where T : ITransaction
        {
            T transaction;
            if (savedTransactions.TryGetValue(typeof(T), out ITransaction temp)) return (T) temp;
            if (TryCreateTransactionWithConstructor(out transaction)) return transaction;
            if (TryCreateTransactionWithStrategy(out transaction)) return transaction;

            return default;
        }

        /// <summary>
        /// Adds a new strategy for instantiating a transaction.
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="setMode">When selected SetMode.Force will replace the current strategy, for the type, if it exists.</param>
        public virtual void AddStrategy(ITransactionStrategy strategy, SetMode setMode = SetMode.None)
        {
            if (CheckNewStrategy(strategy, setMode))
                strategies.Add(strategy.TransactionType, strategy);
        }

        /// <summary>
        /// Remove transactions of type T.
        /// </summary>
        /// <param name="type">Transaction type.</param>
        public void RemoveTransaction(Type type)
        {
            if (savedTransactions.ContainsKey(type)) 
                savedTransactions.Remove(type);
        }

        #endregion
        
        #region Methods

        protected virtual  bool TryCreateTransactionWithConstructor<T>(out T transaction)
            where T : ITransaction
        {
            Type transactionType = typeof(T);
            bool canCreate = transactionType.GetConstructors().Length > 0;
            transaction = canCreate ? Activator.CreateInstance<T>() : default;
            SaveTransaction(transaction);
            return canCreate && transaction.NotNull();
        }

        protected virtual  bool TryCreateTransactionWithStrategy<T>(out T transaction)
            where T : ITransaction
        {
            Type transactionType = typeof(T);
            bool canCreate = strategies.TryGetValue(transactionType, out ITransactionStrategy strategy);
            if (canCreate)
            {
                transaction = (T) strategy.CreateInstance();
                SaveTransaction(transaction);
            }
            else
            {
                transaction = default;
                LogWarning(HasNoStrategy(transactionType));
            }
            return canCreate;
        }

        protected virtual  bool CheckNewStrategy(ITransactionStrategy strategy, SetMode setMode)
        {
            if (strategy.IsNull())
            {
                LogWarning(StrategyIsNull);
                return false;
            }

            Type transactionType = strategy.TransactionType;
            if (transactionType.IsNull())
            {
                LogWarning(TypeIsNull);
                return false;
            }

            if (!transactionType.HasInterface<ITransaction>())
            {
                LogWarning(HasNoITransactionInterface(transactionType));
                return false;
            }

            if (strategies.ContainsKey(transactionType) && setMode != SetMode.Force)
            {
                LogWarning(ContainsStrategy(transactionType));
                return false;
            }

            return true;
        }

        protected virtual void SaveTransaction<T>(T transaction) where T : ITransaction
        {
            if (transaction.IsNull()) return;
            savedTransactions.Add(typeof(T), transaction);
        }

        protected virtual  void LogWarning(string message)
        {
            if (IsLoggerActive) Debug.LogWarning(message);
        }

        protected virtual  string HasNoStrategy(Type transactionType) => 
            $"No strategy added to create transaction type {transactionType.FullName}, use {AddStrategyMethodName} method.";
        
        protected virtual  string ContainsStrategy(Type transactionType) => 
            $"{nameof(strategies)} already contains a definition for transaction type {transactionType}, use {nameof(SetMode.Force)} if you want to replace strategy.";

        protected virtual  string HasNoITransactionInterface(Type currentType) =>
            $"Type {currentType.FullName} does not implement interface {nameof(ITransaction)}";

        #endregion
    }
}