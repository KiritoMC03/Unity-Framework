using System.Collections;
using Framework.Base.Extensions;
using UnityEngine;

namespace Framework.Idlers.TutorialModule
{
    public class TutorialIndicator : MonoBehaviour, ITutorialIndicator
    {
        #region Fields

        [SerializeField]
        protected CanvasGroup canvasGroup;

        [SerializeField]
        private bool ignoreDifferenceByY = true;
        
        protected new Camera camera;
        
        protected Transform cameraTransform;
        protected Transform indicatorAnchor;
        protected Transform targetTransform;

        protected Vector3 indicatorRotation;

        protected bool isActive;
        protected bool reactivateAfterEnabled;

        #endregion

        #region Unity lifecycle

        protected virtual void OnEnable()
        {
            if (!reactivateAfterEnabled || targetTransform.IsNull()) return;
            reactivateAfterEnabled = false;
            StartIndication(targetTransform);
        }

        protected virtual void OnDisable()
        {
            if (!isActive) return;
            reactivateAfterEnabled = true;
            StopIndication();
        }

        #endregion

        #region Methods

        public virtual void SetupIndicatorRotation()
        {
            float angle = CalculateAngleToTarget();
            indicatorRotation.z = angle;
            gameObject.transform.rotation = Quaternion.Euler(indicatorRotation);
        }

        protected virtual float CalculateAngleToTarget(Transform anchor = default)
        {
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 anchorPosition = anchor.NotNull() ? anchor.position : indicatorAnchor.position;
            Vector3 targetPosition = targetTransform.position;
            
            Vector3 differenceToTarget = targetPosition - anchorPosition;
            if (ignoreDifferenceByY) differenceToTarget = differenceToTarget.SetY(0.0f);
            Vector3 direction = differenceToTarget.normalized;
            
            Vector3 forward = new Vector3(cameraForward.x, 0f, cameraForward.z);
            return Vector3.SignedAngle(direction, forward, Vector3.up);
        }

        public virtual void UpdateIndicatorView(Transform anchor = default)
        {
            if (canvasGroup.alpha <= 0f)
                canvasGroup.alpha = 1f;
            SetupIndicatorRotation();
        }

        protected virtual bool CheckTargetInViewScope(Vector3 targetPositionOnScreen) => 
            targetPositionOnScreen.x > 0 && targetPositionOnScreen.x < 1 && 
            targetPositionOnScreen.y > 0 && targetPositionOnScreen.y < 1 && 
            targetPositionOnScreen.z > 0;

        #endregion

        #region Coroutines

        protected virtual IEnumerator UpdateIndicatorRoutine(Transform anchor = default)
        {
            while (true)
            {
                if (!isActive) continue;
                Vector3 targetPositionOnScreen = camera.WorldToViewportPoint(targetTransform.position);
                if (CheckTargetInViewScope(targetPositionOnScreen))
                {
                    if (canvasGroup.alpha >= 1f)
                        canvasGroup.alpha = 0f;
                }
                else UpdateIndicatorView();

                yield return null;
            }
        }

        #endregion

        #region ITutorialIndicator

        public GameObject GameObject => gameObject;
        public Transform Transform => transform;

        public virtual void Init(Camera camera, Transform indicatorAnchor)
        {
            this.camera = camera;
            this.indicatorAnchor = indicatorAnchor;
            cameraTransform = camera.transform;
        }

        public virtual void StartIndication(Transform targetTransform)
        {
            this.targetTransform = targetTransform;
            isActive = true;
            StartCoroutine(UpdateIndicatorRoutine());
        }

        public virtual void StopIndication()
        {
            isActive = false;
            canvasGroup.alpha = 0f;
        }

        public virtual void Destroy()
        {
            StopIndication();
            StopCoroutine(UpdateIndicatorRoutine());
            gameObject.DestroyNotNull();
        }

        #endregion
    }
}