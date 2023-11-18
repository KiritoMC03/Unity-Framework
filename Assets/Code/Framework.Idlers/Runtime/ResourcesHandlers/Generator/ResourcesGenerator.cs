using System;
using System.Collections;
using Framework.Base.Extensions;
using Framework.Idlers.Resource;
using UnityEngine;

namespace Framework.Idlers.ResourcesHandlers
{
    public class ResourcesGenerator : MonoBehaviour
    {
        #region Events

        public event Action GenerationStartedCallback;
        public event Action GenerationStoppedCallback;
        public event Action<IBaseResource> GeneratedCallback;
        /// <summary>
        /// Invoke with generation progress between 0 and 1
        /// </summary>
        public event Action<float> GeneratingProcessedCallback; 

        #endregion
        
        #region Fields

        public ResourceType generatingType;
        public float generationDuration;
        public Transform spawnPoint;
        
        protected TryGetResourceDelegate getResourceFunc;
        protected Coroutine routine;
        protected const string DontGeneratedMessage = "Resource do not generated.";
        protected bool isInitialized;

        #endregion

        #region Properties

        public virtual bool IsGenerating { get; protected set; }
        public virtual bool NeedMoveGenerated { get; protected set; } = true;

        #endregion

        #region Methods


        public virtual void Init(TryGetResourceDelegate getResourceDelegate)
        {
            getResourceFunc = getResourceDelegate;
            isInitialized = true;
        }

        public virtual void StartGenerating()
        {
            if (!CheckInitialized()) return;
            StopGenerating();
            routine = StartCoroutine(GenerateRoutine());
        }

        public virtual void StopGenerating()
        {
            if (routine.NotNull())
            {
                StopCoroutine(routine);
                routine = default;
            }
            
            IsGenerating = false;
            GenerationStoppedCallback?.Invoke();
        }

        public virtual IBaseResource Generate()
        {
            if (!CheckInitialized()) return default;
            if (!getResourceFunc.Invoke(generatingType, out IBaseResource resource))
            {
                Debug.LogWarning(DontGeneratedMessage);
                return default;
            }

            if (NeedMoveGenerated) MoveGenerated(resource);
            GeneratedCallback?.Invoke(resource);
            return resource;
        }

        protected virtual void MoveGenerated(IBaseResource resource)
        {
            if (resource.IsNull()) return;
            resource.CacheTransform.position = spawnPoint.position;
            resource.CacheTransform.rotation = spawnPoint.rotation;
        }

        protected virtual bool CheckInitialized()
        {
            if (isInitialized) return true;
            Debug.LogWarning($"{typeof(ResourcesGenerator)} is not initialized.", this);
            return false;
        }

        protected virtual void InvokeGenerationStartedCallback() => GenerationStartedCallback?.Invoke();
        protected virtual void InvokeGenerationStoppedCallback() => GenerationStoppedCallback?.Invoke();
        protected virtual void InvokeGeneratedCallback(IBaseResource resource) => GeneratedCallback?.Invoke(resource);
        protected virtual void InvokeGeneratingProcessedCallback(float progress) => GeneratingProcessedCallback?.Invoke(progress);

        #endregion

        #region Coroutines

        protected virtual IEnumerator GenerateRoutine()
        {
            IsGenerating = true;
            while (IsGenerating)
            {
                GenerationStartedCallback?.Invoke();
                for (float i = 0f; i < generationDuration; i += Time.deltaTime)
                {
                    GeneratingProcessedCallback?.Invoke(i / generationDuration);
                    yield return null;
                }
                
                GeneratingProcessedCallback?.Invoke(0f);
                Generate();
            }
        }

        #endregion
    }
}