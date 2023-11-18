using Framework.Base.Extensions;
using Framework.Idlers.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Idlers.UI
{
    public class ProgressBar : MonoBehaviour
    {
        #region Fields
        
        [SerializeField]
        protected Image image;

        [SerializeField]
        protected Gradient colorInterpolation;

        [SerializeField]
        protected float defaultFillTime;

        private Coroutine coroutine;
        
        #endregion
        
        #region Properties

        public ProgressBarState State { get; protected set; }
        public Transform Transform { get; protected set; }

        #endregion

        #region Methods
        
        public virtual void Init() => this.Transform = transform;
        public virtual void Init(float defaultFillTime)
        {
            this.defaultFillTime = defaultFillTime;
            Init();
        }

        protected virtual void RefreshProgress(float percent)
        {
            image.fillAmount = percent;
            image.color = colorInterpolation.Evaluate(percent);
            if (percent < 1) return;
            State = ProgressBarState.Filled;
            KillRoutine();
        }

        protected virtual void KillRoutine()
        {
            if (coroutine.NotNull()) StopCoroutine(coroutine);
        }

        #endregion

        #region ITrackableProgressBar

        public void StartFill()
        {
            if (coroutine.NotNull()) return;
            gameObject.SetActive(true);
            State = ProgressBarState.Filling;
            coroutine = this.FillProgress(0, 1, defaultFillTime, RefreshProgress);
        }
        
        public void Hide()
        {
            KillRoutine();
            State = ProgressBarState.Paused;
            gameObject.SetActive(false);
        }
        
        public void Destroy()
        {
            KillRoutine();
            gameObject.DestroyNotNull();
        }

        #endregion
    }
}