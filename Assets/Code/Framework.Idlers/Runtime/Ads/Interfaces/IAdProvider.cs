using System;
using Framework.Base.Dependencies.Mediator;

namespace Framework.Idlers.Ads
{
    public interface IAdProvider : ISingleComponent
    {
        public void ShowBanner();
        public void CacheInterstitial();
        public void CacheRewarded();
        public void ShowInterstitial(Action completeAction);
        public void ShowRewarded(Action completedAction, Action failedAction);
    }
}