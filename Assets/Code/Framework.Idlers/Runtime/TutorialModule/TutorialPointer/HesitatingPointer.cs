using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Framework.Base.Extensions;
using UnityEngine;

namespace Framework.Idlers.TutorialModule
{
    public class HesitatingPointer : MonoBehaviour, ITutorialPointer
    {
        #region Fields

        [SerializeField]
        protected Vector3 hesitationOffset;

        [SerializeField]
        protected float hesitationDuration;
        
        [SerializeField]
        protected Ease ease;

        protected TweenerCore<Vector3, Vector3, VectorOptions> currentTweener;

        #endregion

        #region Properties

        public virtual GameObject GameObject => gameObject;

        #endregion

        #region Methods

        protected virtual void CheckForNull()
        {
            if (currentTweener.target == null)
            {
                KillTweener();
            }
        }

        protected virtual void KillTweener()
        {
            if (currentTweener != null)
                currentTweener.Kill();
        }

        #endregion
        
        #region Methods

        public virtual void StartHesitation(Vector3 position)
        {
            Show();
            transform.position = position;
            currentTweener = transform.DOMoveY(transform.position.y + hesitationOffset.y, hesitationDuration)
                .SetLoops(-1, DG.Tweening.LoopType.Yoyo)
                .SetEase(ease)
                .SetLink(gameObject);
            currentTweener.onUpdate += CheckForNull;
        }

        public virtual void StopHesitation()
        {
            transform.localScale = Vector3.zero;
            KillTweener();
        }

        public virtual void Show() => gameObject.SetActive(true);
        public virtual void Hide() => gameObject.SetActive(false);
        public virtual void Destroy() => gameObject.DestroyNotNull();

        #endregion
    }
}