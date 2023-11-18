using System;
using Framework.Base.Dependencies.Mediator;

namespace Framework.Idlers.TutorialModule
{
    [Serializable]
    public class TutorialDB : ISingleComponent
    {
        public int tutorialPartIndex;
    }
}