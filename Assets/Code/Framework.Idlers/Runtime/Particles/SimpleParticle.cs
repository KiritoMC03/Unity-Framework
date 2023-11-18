using System;
using System.Collections;
using Framework.Base;
using Framework.Base.Extensions;
using Framework.Idlers.Extensions;
using UnityEngine;

namespace Framework.Idlers.Particles
{
    public class SimpleParticle : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        protected ParticleSystem system;

        [SerializeField]
        protected bool overrideDuration;

        [SerializeField] [Min(0.0f)] [ShowIf(nameof(overrideDuration))]
        protected float duration;

        protected Coroutine followRoutine;
        protected bool isAttached;
        protected Transform attachTarget;
        protected Vector3 attachOffset;

        #endregion

        #region Properties

        public virtual ParticleSystem System => system;
        public virtual bool OverrideDuration => overrideDuration;
        public virtual float Duration => duration;

        #endregion

        #region Methods

        private void OnEnable()
        {
            if (isAttached) followRoutine = StartCoroutine(FollowRoutine(attachTarget, attachOffset));
        }

        #endregion

        #region Methods

        public virtual SimpleParticle Show()
        {
            gameObject.SetActive(true);
            return this;
        }

        public virtual SimpleParticle Hide()
        {
            gameObject.SetActive(false);
            return this;
        }

        public virtual SimpleParticle Play()
        {
            system.Play();
            return this;
        }

        public virtual void InvokeOnComplete(Action action)
        {
            if (!overrideDuration) duration = GetComponent<ParticleSystem>().main.duration;
            this.InvokeWithDelay(action, duration);
        }

        public virtual void InvokeOnComplete(Action<SimpleParticle> action)
        {
            if (!overrideDuration) duration = GetComponent<ParticleSystem>().main.duration;
            this.InvokeWithDelay(() => action?.Invoke(this), duration);
        }

        public virtual void InvokeOnComplete(Action<GameObject> action)
        {
            if (!overrideDuration) duration = GetComponent<ParticleSystem>().main.duration;
            this.InvokeWithDelay(() => action?.Invoke(gameObject), duration);
        }

        public virtual void InvokeOnComplete(Action<Transform> action)
        {
            if (!overrideDuration) duration = GetComponent<ParticleSystem>().main.duration;
            this.InvokeWithDelay(() => action?.Invoke(transform), duration);
        }
        
        public virtual void Attach(Transform target, Vector3 offset) => followRoutine = StartCoroutine(FollowRoutine(target, offset));
        public virtual void Attach(Transform target) => Attach(target, Vector3.zero);
        public virtual void Attach(GameObject target) => Attach(target.transform, Vector3.zero);
        public virtual void Attach(GameObject target, Vector3 offset) => Attach(target.transform, Vector3.zero);
        public virtual void Attach(MonoBehaviour target) => Attach(target.transform, Vector3.zero);
        public virtual void Attach(MonoBehaviour target, Vector3 offset) => Attach(target.transform, Vector3.zero);

        public virtual void Detach()
        {
            isAttached = false;
            if (followRoutine != null)
                StopCoroutine(followRoutine);
            followRoutine = default;
        }

        #endregion

        #region Coroutines

        protected virtual IEnumerator FollowRoutine(Transform target, Vector3 offset)
        {
            this.attachTarget = target;
            this.attachOffset = offset;
            this.isAttached = true;
            while (target.NotNull())
            {
                transform.position = target.position + offset;
                yield return null;
            }
            isAttached = false;
        }

        #endregion
    }
}