using System;
using System.Collections.Generic;
using GameKit.CraftModule.Resource;
using UnityEngine;

namespace GameKit.CraftModule.Zones
{
    public interface IResourceStorageZone
    {
        #region Events
        
        public event Action ReceivedCallback;
        public event Action SentCallback;

        #endregion
        
        #region Properties

        public bool IsInitialized { get; }
        public int Capacity { get; set; }
        public int ObjectsCount { get; }
        public bool CanReceive { get; }
        public bool HasObject { get; }
        public List<IBaseResource> ContainedObjects { get; }

        #endregion

        #region Methods

        public void Init(List<IBaseResource> buffer, int capacity);
        public bool TryReceive(IBaseResource target);
        public bool TryPop(out IBaseResource target);
        public void SetAcceptedResourceTypes(List<ResourceType> types);
        public Vector3 GetItemPosition();
        public Vector3 GetItemPosition(int index);
        public void ValidateResourcesPositions();

        #endregion
    }
}