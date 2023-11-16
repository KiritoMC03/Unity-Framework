using System.Collections;
using UnityEngine;

namespace GameKit.TutorialModule
{
    public interface ITutorialIndicator
    {
        #region Fields

        public GameObject GameObject { get; }
        public Transform Transform { get; }

        #endregion

        #region Methods

        public void Init(Camera camera, Transform indicatorAnchor);
        public void StartIndication(Transform target);
        public void StopIndication();
        public void Destroy();

        #endregion
    }
}