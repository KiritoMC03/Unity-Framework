using System;
using System.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Framework.Base.Extensions;
using Framework.Idlers.Buildings;
using Framework.Idlers.Resource;
using UnityEngine;

namespace Framework.Idlers.ResourcesHandlers
{
    public class ByRequestResourcesGenerator : MonoBehaviour, IResourceSender, IBuildingLogic
    {
        #region Fields
        
        [SerializeField]
        protected Vector3 generatingPositionOffset;
        
        protected IResourcesCreator resourcesCreator;

        #endregion

        #region Properties
        
        [field: SerializeField]
        public virtual bool IsGenerateRandomResources { get; set; }

        [field: SerializeField]
        public virtual ResourceType GeneratingResource { get; set; }
        
        [field: SerializeField]
        public virtual ResourceType[] RandomGenerationResources { get; set; }

        #endregion
        
        #region Unity lifecycle

#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position + generatingPositionOffset, 0.1f);
        }
#endif

        #endregion
        
        #region Methods

        public virtual void PreInit() => transform.localScale = Vector3.zero;
        public virtual void Init(IResourcesCreator resourcesCreator) => this.resourcesCreator = resourcesCreator;

        protected virtual IBaseResource GenerateResource()
        {
            if (IsGenerateRandomResources)
                return resourcesCreator.GetResource(RandomGenerationResources.GetRandomItem());
            else
                return resourcesCreator.GetResource(GeneratingResource);
        }

        #endregion

        #region IResourceSender

        public virtual event Action SentCallback;
        public virtual event Action<IBaseResource> ItemSentCallback;
        public virtual bool HasObject { get; private set; } = false;

        public virtual bool TryPop(out IBaseResource target)
        {
            target = GenerateResource();
            target.CacheTransform.position = transform.position + generatingPositionOffset;
            SentCallback?.Invoke();
            ItemSentCallback?.Invoke(target);
            return true;
        }

        #endregion

        #region IBuildingLogic

        public virtual GameObject GameObject => gameObject;
        public virtual Transform Transform => transform;
        public virtual void Init(int id) => HasObject = true;

        public virtual async Task PlayBuiltVisualization(bool doImmediately = false)
        {
            if (doImmediately)
            {
                transform.localScale = Vector3.one;
                return;
            }

            TweenerCore<Vector3, Vector3, VectorOptions> scaleTweener =
                transform.DOScale(Vector3.one, 1f);
            while (scaleTweener.active && !scaleTweener.IsComplete()) await Task.Yield();
        }

        #endregion
    }
}