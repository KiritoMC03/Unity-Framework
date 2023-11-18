using System;
using Framework.Base.Collections;

namespace Framework.Base.Transactions
{
    public abstract class TutorialBasedTransactionStrategy<TInterface, TCommon, TTutorial> : TransactionStrategy
        where TInterface : ITransaction
        where TCommon : TInterface, new()
        where TTutorial : TInterface, new()
    {
        #region Fields

        private readonly ObservableValue<bool> partActivityState;
        private readonly TCommon common = new TCommon();
        private readonly TTutorial tutorial = new TTutorial();

        #endregion

        #region Properties

        public override Type TransactionType => typeof(TInterface);

        #endregion
        
        #region Constructors

        public TutorialBasedTransactionStrategy(ObservableValue<bool> partActivityState)
        {
            this.partActivityState = partActivityState;
            partActivityState.ChangedCallback += HandleStateChanged;
        }

        #endregion

        #region Methods

        public virtual void HandleStateChanged(bool partActive) => 
            LockCurrentTransaction();

        public override ITransaction CreateInstance() => 
            partActivityState.Value ? (ITransaction)tutorial : (ITransaction)common;

        #endregion
    }
}