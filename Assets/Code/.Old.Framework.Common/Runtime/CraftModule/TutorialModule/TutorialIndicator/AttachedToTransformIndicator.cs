using System.Collections;
using GameKit.General.Extensions;
using UnityEngine;

namespace GameKit.TutorialModule
{
    public class AttachedToTransformIndicator : TutorialIndicator
    {
        #region Fields

        [SerializeField]
        private Vector3 positionOffset = new Vector3(0.0f, 0.0f, 1.0f);

        private Quaternion rotationAroundAnchor;

        #endregion

        #region Methods

        protected virtual void SetupIndicatorRotation(Transform anchor)
        {
            float angle = CalculateAngleToTarget(anchor);
            indicatorRotation.y = -angle;
            transform.rotation = Quaternion.Euler(indicatorRotation);
            rotationAroundAnchor = Quaternion.Euler(new Vector3(0f, angle * -1.0f, 0f));
        }

        protected virtual void CalculateIndicatorPosition()
        {
            Vector3 indicatorPosition = indicatorAnchor.TransformPoint(positionOffset.Rotate(rotationAroundAnchor));
            transform.position = indicatorPosition;
        }

        public override void UpdateIndicatorView(Transform anchor)
        {
            SetupIndicatorRotation(anchor);
            CalculateIndicatorPosition();
        }
        
        public override void StartIndication(Transform targetTransform)
        {
            this.targetTransform = targetTransform;
            isActive = true;
            StartCoroutine(UpdateIndicatorRoutine(indicatorAnchor));
        }

        #endregion

        #region Coroutines
        
        protected override IEnumerator UpdateIndicatorRoutine(Transform anchor)
        {
            while (true)
            {
                if (!isActive) yield break;
                UpdateIndicatorView(anchor);
                yield return null;
            }
        }

        #endregion
    }
}