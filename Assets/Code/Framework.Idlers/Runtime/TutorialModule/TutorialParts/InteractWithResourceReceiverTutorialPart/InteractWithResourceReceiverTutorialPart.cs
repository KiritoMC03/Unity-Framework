using System;
using System.Collections.Generic;
using Framework.Base.Dependencies.Mediator;
using Framework.Base.Extensions;
using Framework.Idlers.Resource;
using UnityEngine;

namespace Framework.Idlers.TutorialModule
{
    public class InteractWithResourceReceiverTutorialPart : MonoBehaviour, ITutorialPart
    {
        #region Fields

        [SerializeField]
        protected InteractWithResourceReceiverTutorialPartData data;
        
        protected Dictionary<ResourceType, int> listOfResourcesToReceive;
        protected ITutorialPointer pointer;

        #endregion

        #region Methods

        protected virtual void HandleReceivedResource(IBaseResource resource)
        {
            if (resource.IsNull()) return;
            ResourceType resourceType = resource.Type;
            if (!listOfResourcesToReceive.ContainsKey(resourceType)) return;
            listOfResourcesToReceive[resourceType]--;
            if (listOfResourcesToReceive[resourceType] <= 0) 
                listOfResourcesToReceive.Remove(resourceType);
            if (listOfResourcesToReceive.Count == 0) Complete();
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

        public event Action CompletedCallback;

        public virtual IReadOnlyList<Type> NeededTutorialDataStorageTypes { get; } = new List<Type>
        {
            typeof(InteractWithResourceReceiverTutorialPartData),
        };

        public virtual bool TryPrepare()
        {
            data.Validate();
            listOfResourcesToReceive = new Dictionary<ResourceType, int>(data.listOfResourcesToReceive);
            return true;
        }

        public virtual bool TryStart()
        {
            pointer = ShowTutorialPointer(data.resourceReceiverComponent.transform.position);
            data.resourceReceiver.ItemReceivedCallback += HandleReceivedResource;
            return true;
        }

        public virtual void Complete()
        {
            data.resourceReceiver.ItemReceivedCallback -= HandleReceivedResource;
            pointer?.Destroy();
            data = default;
            listOfResourcesToReceive = default;
            CompletedCallback?.Invoke();
        }

        public virtual void AfterCompleteAllParts() { }
        
        public virtual bool TryInsertData<TData>(TData inputData) where TData : ITutorialDataStorage
        {
            if (inputData.NotValid<InteractWithResourceReceiverTutorialPartData>()) return false;
            data = inputData as InteractWithResourceReceiverTutorialPartData;
            listOfResourcesToReceive = new Dictionary<ResourceType, int>(listOfResourcesToReceive);
            return true;
        }

        #endregion
    }
}