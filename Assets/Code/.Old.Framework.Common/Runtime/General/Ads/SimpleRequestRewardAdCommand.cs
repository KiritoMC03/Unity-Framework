using System;
using GameKit.General.TransactionSystem;
using General.Extensions;
using General.Mediator;

namespace GameKit.General.Ads
{
    public class SimpleRequestRewardAdCommand : IRequestRewardAdCommand
    {
        #region Fields

        private IAdProvider provider;

        #endregion
        
        #region Methods

        protected virtual IAdProvider GetProvider()
        {
            MC.Instance.GetSingleComponent(this, out ITransactionSystem transactionSystem);
            IAdTransaction result = transactionSystem.GetTransaction<IAdTransaction>();
            if (result.NotNull()) return result;
            MC.Instance.GetSingleComponent(this, out IAdProvider adProvider);
            return adProvider;
        }

        #endregion
        
        #region IRequestRewardAdCommand

        public virtual void Request(Action completedAction, Action failedAction)
        {
            if (provider.IsNull()) provider = GetProvider();
            provider.ShowRewarded(completedAction, failedAction);
        }

        #endregion
    }
}