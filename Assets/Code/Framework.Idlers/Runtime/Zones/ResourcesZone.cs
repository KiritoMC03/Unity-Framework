using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Framework.Base;
using Framework.Base.Extensions;
using Framework.Idlers.Extensions;
using Framework.Idlers.Resource;
using UnityEngine;

namespace Framework.Idlers.Zones
{
    public class ResourcesZone : MonoBehaviour, IResourceStorageZone
    {
        #region Events IResourceReceiver
        
        public event Action ReceivedCallback;
        public event Action<IBaseResource> ItemReceivedCallback;
        public event Action SentCallback;
        public event Action<IBaseResource> ItemSentCallback;

        #endregion

        #region Fields

        [SerializeField]
        protected List<ResourceType> acceptedResourceTypes = new List<ResourceType>();

        [SerializeField]
        protected GridZonePattern.Preferences defaultPatternPreferences;

        [SerializeField]
        protected float resourceJumpDuration = 0.2f;

        [SerializeField]
        protected bool useItemsLocalRotation;
        
        [SerializeField] [ShowIf(nameof(useItemsLocalRotation))]
        protected Vector3 itemsLocalRotation;
        
        protected BaseStorageZoneLogic<IBaseResource> logic;
        protected GridZonePattern defaultPattern;
        protected int fliedInResourcesNumber;

        #endregion

        #region Properties IResourceStorageZone

        public virtual int Capacity
        {
            get => logic.Capacity;
            set => logic.RefreshCapacity(value);
        }

        public bool IsInitialized { get; protected set; }
        public virtual int ObjectsCount => logic.ObjectsCount;
        public virtual bool CanReceive => ObjectsCount + fliedInResourcesNumber < Capacity;
        public virtual bool HasObject => logic.HasObject;
        public virtual List<IBaseResource> ContainedObjects => logic.ContainedObjects;
        public float ResourceJumpDuration => resourceJumpDuration;

        #endregion

        #region Methods IResourceReceiver

        public virtual IEnumerable<ResourceType> GetAcceptedMatchers() => acceptedResourceTypes;
        
        #endregion

        #region Methods: IResourceReceiver, IResourceStorageZone

        public virtual bool TryReceive(IBaseResource target)
        {
            if (!CanReceive) return false;
            if (target.IsNull()) return false;

            Vector3 pos = GetItemPosition();
            TweenerCore<Quaternion, Vector3, QuaternionOptions> rotationTweener;
            fliedInResourcesNumber++;
            StartItemRotation(target, out rotationTweener);
            Sequence tweener = target.JumpTo(pos, resourceJumpDuration);
            tweener.onComplete += Receive;

            return true;
            void Receive() => ReceiveInternal(target, rotationTweener);
        }

        private void ReceiveInternal(IBaseResource target, TweenerCore<Quaternion, Vector3, QuaternionOptions> rotationTweener)
        {
            fliedInResourcesNumber--;
            if (useItemsLocalRotation) rotationTweener?.Kill();
            Quaternion rotation = defaultPatternPreferences.center.rotation;
            if (useItemsLocalRotation) rotation *= Quaternion.Euler(itemsLocalRotation);
            target.CacheTransform.rotation = rotation;
            logic.TryReplenish(target);
            InvokeReceivedCallback(target);
        }

        public virtual bool TryPop(out IBaseResource target)
        {
            bool result = logic.TryPopObject(out target);
            if (result) InvokeSentCallback(target);
            
            return result;
        }

        #endregion

        #region Methods

        public virtual void Init(List<IBaseResource> buffer, int capacity)
        {
            defaultPattern = new GridZonePattern(defaultPatternPreferences);
            logic = new CommonStorageZoneLogic<IBaseResource>(buffer, capacity, defaultPattern);
            IsInitialized = true;
            ValidateResourcesPositions();
        }

        public virtual void SetAcceptedResourceTypes(List<ResourceType> types) => acceptedResourceTypes = types;

        public virtual Vector3 GetItemPosition() => GetItemPosition(ObjectsCount + fliedInResourcesNumber);
        public virtual Vector3 GetItemPosition(int index) => logic.GetItemPosition(index);
        
        public virtual void ValidateResourcesPositions()
        {
            List<IBaseResource> items = logic.ContainedObjects;
            Quaternion rotation = defaultPatternPreferences.center.rotation;
            if (useItemsLocalRotation) rotation *= Quaternion.Euler(itemsLocalRotation);
            for (int i = 0; i < items.Count; i++)
            {
                items[i].CacheTransform.position = GetItemPosition(i);
                items[i].CacheTransform.rotation = rotation;
            }
        }
        
        protected void InvokeReceivedCallback(IBaseResource resource)
        {
            ItemReceivedCallback?.Invoke(resource);
            ReceivedCallback?.Invoke();
        }

        protected void InvokeSentCallback(IBaseResource resource)
        {
            ItemSentCallback?.Invoke(resource);
            SentCallback?.Invoke();
        }

        protected virtual void StartItemRotation(IBaseResource target, out TweenerCore<Quaternion, Vector3, QuaternionOptions> rotationTweener)
        {
            rotationTweener = default;
            if (!useItemsLocalRotation) return;
            Vector3 targetRotation = defaultPatternPreferences.center.rotation * itemsLocalRotation;
            rotationTweener = target.CacheTransform.DORotate(targetRotation, resourceJumpDuration);
        }

        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Dictionary<int, Color> colorsDic = new Dictionary<int, Color> {{0, Color.gray}, {1, Color.green}, {2, Color.blue}};
            int cellsNumber = defaultPatternPreferences.xRowSize * defaultPatternPreferences.zRowSize;
            GridZonePattern backpackPattern = new GridZonePattern(defaultPatternPreferences);

            Quaternion rotation = defaultPatternPreferences.center.rotation;
            if (useItemsLocalRotation)
                rotation *= Quaternion.Euler(itemsLocalRotation);
            
            for (int i = 0; i < cellsNumber; i++)
            {
                Gizmos.color = colorsDic[i % 3];
                var position = backpackPattern.GetPosition(i) - defaultPatternPreferences.pivotOffset;
                Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
                Gizmos.matrix = matrix;
                Gizmos.DrawWireCube(Vector3.zero, defaultPatternPreferences.cellSize);
            }
        }
#endif
    }
}