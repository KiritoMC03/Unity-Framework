using Framework.Base.Dependencies.Mediator;
using UnityEngine;

namespace Framework.Idlers.TutorialModule
{
    public interface ITutorialPointer : ISingleComponent
    {
        public GameObject GameObject { get; }

        public void StartHesitation(Vector3 position);
        public void StopHesitation();
        public void Show();
        public void Hide();
        public void Destroy();
    }
}