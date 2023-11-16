using General.Mediator;
using UnityEngine;

namespace GameKit.TutorialModule
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