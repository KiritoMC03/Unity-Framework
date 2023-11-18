using System;
using System.Reflection;
using Framework.Base.Dependencies.Mediator;
using UnityEngine;

namespace Framework.Base.Transactions
{
    public class Transaction : ITransaction
    {
        #region Fields

        private ObserverSingleComponent<ITransactionSystem> transactions;

        #endregion

        #region Constructors

        public Transaction()
        {
            transactions = new ObserverSingleComponent<ITransactionSystem>(this);
        }

        #endregion
        
        #region Properties

        protected ITransactionSystem Transactions => transactions.Result;

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