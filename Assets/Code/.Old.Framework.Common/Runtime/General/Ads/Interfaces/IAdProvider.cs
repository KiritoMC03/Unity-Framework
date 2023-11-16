using System;
using General.Mediator;

namespace GameKit.General.Ads
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