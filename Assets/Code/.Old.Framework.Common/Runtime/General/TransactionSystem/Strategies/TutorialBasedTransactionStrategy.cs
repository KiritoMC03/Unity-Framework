using System;
using General;

namespace GameKit.General.TransactionSystem
{
    public abstract class TutorialBasedTransactionStrategy<TInterface, TCommon, TTutorial> : TransactionStrategy
        where TInterface : ITransaction
        where TCommon : TInterface, new()
        where TTutorial : TInterface, new()
    {
        #region Fields

        private readonly ObservableObject<bool> partActivityState;
        private TCommon common;
        private TTutorial tutorial;

        #endregion

        #region Properties

        public override Type TransactionType => typeof(TInterface);

        #endregion
        
        #region Constructors

        public TutorialBasedTransactionStrategy(ObservableObject<bool> partActivityState)
        {
            this.partActivityState = partActivityState;
            this.common = new TCommon();
            this.tutorial = new TTutorial();
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