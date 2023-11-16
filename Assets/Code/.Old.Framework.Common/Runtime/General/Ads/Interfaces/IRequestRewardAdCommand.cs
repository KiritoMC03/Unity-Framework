using System;

namespace GameKit.General.Ads
{
    public interface IRequestRewardAdCommand
    {
        public void Request(Action completedAction, Action failedAction);
    }
}