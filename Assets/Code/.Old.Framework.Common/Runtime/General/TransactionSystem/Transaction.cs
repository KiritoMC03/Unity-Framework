using System;
using System.Reflection;
using GameKit.General.LocalDB;
using General.Mediator;
using UnityEngine;

namespace GameKit.General.TransactionSystem
{
    public class Transaction : ITransaction
    {
        #region Fields

        private ObserverSingleComponent<ITransactionSystem> transactions;

        private ObserverSingleComponent<ConfigsDB> configsDB;

        #endregion

        #region Constructors

        public Transaction()
        {
            transactions = new ObserverSingleComponent<ITransactionSystem>(this);
            configsDB = new ObserverSingleComponent<ConfigsDB>(this);
        }

        #endregion
        
        #region Properties

        protected ITransactionSystem Transactions => transactions.Result;
        protected ConfigsDB ConfigsDB => configsDB.Result;

        #endregion
        
        #region Methods

        protected virtual T GetTransaction<T>() where T : ITransaction => 
            transactions.Result.GetTransaction<T>();
        
        protected void InitObserversWithThis()
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            FieldInfo[] fields = GetType().GetFields(flags);
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.Name == typeof(ObserverSingleComponent<>).Name)
                {
                    field.SetValue(this, Activator.CreateInstance(field.FieldType, this));
                }
                else
                {
                    Debug.Log("");
                }
            }
        }

        #endregion
    }
}