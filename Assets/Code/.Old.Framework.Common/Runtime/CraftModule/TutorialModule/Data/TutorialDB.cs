using System;
using General.Mediator;

namespace GameKit.TutorialModule
{
    [Serializable]
    public class TutorialDB : ISingleComponent
    {
        public int tutorialPartIndex;
    }
}