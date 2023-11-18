using System;
using Framework.Base.Dependencies.Mediator;

namespace Framework.Idlers.TutorialModule
{
    public interface ITutorialController : ISingleComponent
    {
        public event Action StartedCallback;
        public event Action CompletedCallback;
        public event Action<ITutorialPart> PartStartedCallback;
        public event Action<ITutorialPart> PartCompletedCallback;
        
        public void Init();
        public void Run();
    }
}