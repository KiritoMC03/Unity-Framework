using System;
using System.Collections.Generic;
using Framework.Base.Dependencies.Mediator;
using Framework.Base.Extensions;
using Framework.Idlers.Resource;
using UnityEngine;

namespace Framework.Idlers.TutorialModule
{
    public class InteractWithResourceSenderTutorialPart : MonoBehaviour, ITutorialPart
    {
        #region Fields

        [SerializeField]
        protected InteractWithResourceSenderTutorialPartData data;
        
        protected Dictionary<ResourceType, int> listOfResourcesToSend;
        protected ITutorialPointer pointer;

        #endregion

        #region Methods

        protected virtual void HandleSentResource(IBaseResource resource)
        {
            if (resource.IsNull()) return;
            ResourceType resourceType = resource.Type;
            if (!listOfResourcesToSend.ContainsKey(resourceType)) return;
            listOfResourcesToSend[resourceType]--;
            if (listOfResourcesToSend[resourceType] <= 0) 
                listOfResourcesToSend.Remove(resourceType);
            if (listOfResourcesToSend.Count == 0) Complete();
        }

        protected virtual ITutorialPointer FindTutorialPointerPrefab()
        {
            MC.Instance.GetSingleComponent(this, out ITutorialPointer prefab);
            return prefab;
        }

        protected virtual ITutorialPointer ShowTutorialPointer(Vector3 position)
        {
            ITutorialPointer prefab = FindTutorialPointerPrefab();
            if (prefab.IsNull())
            {
                Debug.LogWarning($"{typeof(ITutorialPointer)} not found.");
                return default;
            }
            ITutorialPointer pointer = Instantiate(prefab.GameObject).GetComponent<ITutorialPointer>();
            pointer.StartHesitation(position);
            return pointer;
        }

        #endregion
        
        #region ITutorialPart

        public virtual event Action CompletedCallback;

        public virtual IReadOnlyList<Type> NeededTutorialDataStorageTypes { get; } = new List<Type>
        {
            typeof(InteractWithResourceSenderTutorialPartData)
        };

        public virtual bool TryPrepare()
        {
            data.Validate();
            listOfResourcesToSend = new Dictionary<ResourceType, int>(data.listOfResourcesToSend);
            return true;
        }

        public virtual bool TryStart()
        {
            pointer = ShowTutorialPointer(data.resourceSenderComponent.transform.position);
            data.resourceSender.ItemSentCallback += HandleSentResource;
            return true;
        }

        public virtual void Complete()
        {
            data.resourceSender.ItemSentCallback -= HandleSentResource;
            pointer?.Destroy();
            data = default;
            listOfResourcesToSend = default;
            CompletedCallback?.Invoke();
        }

        public virtual void AfterCompleteAllParts() { }
        
        public virtual bool TryInsertData<TData>(TData inputData) where TData : ITutorialDataStorage
        {
            if (inputData.NotValid<InteractWithResourceSenderTutorialPartData>()) return false;
            data = inputData as InteractWithResourceSenderTutorialPartData;
            listOfResourcesToSend = new Dictionary<ResourceType, int>(listOfResourcesToSend);
            return true;
        }

        #endregion
    }
}