using System;

namespace Framework.Idlers.Ads
{
    public interface IRequestRewardAdCommand
    {
        public void Request(Action completedAction, Action failedAction);
    }
}