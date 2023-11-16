using System.Collections.Generic;
using System.Linq;
using General.Extensions;
using UnityEngine;

namespace GameKit.CraftModule.Zones
{
    public class CommonStorageZoneLogic<TContent> : BaseStorageZoneLogic<TContent>
    {
        #region Fields

        private List<TContent> containedObjects;
        private List<Vector3> containedObjectsPositions;

        private GridZonePattern pattern;

        #endregion

        #region Properties
        
        public override int ObjectsCount => containedObjects.Count;
        public override bool CanReplenish => containedObjects.Count < Capacity;
        public override bool HasObject => containedObjects.Count > 0;
        public override List<TContent> ContainedObjects => containedObjects;
        protected List<Vector3> ContainedObjectsPositions => containedObjectsPositions;

        #endregion

        #region Constructors

        public CommonStorageZoneLogic(List<TContent> containedObjectsListLink, int capacity, GridZonePattern pattern)
        {
            if (containedObjectsListLink.LogIfNull()) return;
            this.containedObjects = containedObjectsListLink;
            this.containedObjectsPositions = new List<Vector3>();
            this.pattern = pattern;
            this.Capacity = capacity;
            isInitialized = true;
        }

        #endregion

        #region Methods
        
        public override bool TryReplenish(TContent target)
        {
            if (!CheckInitialized() || containedObjects.Count >= Capacity) return false;

            containedObjects.Add(target);
            InvokeReplenishedCallback();
            return true;
        }
        
        public override bool TryPopObject(out TContent result)
        {
            result = default;
            if (!CheckInitialized() || containedObjects.Count < 1) return false;

            result = containedObjects.Last();
            containedObjects.RemoveLastItem();
            InvokeSentCallback();
            return true;
        }

        public override Vector3 GetItemPosition(int index = -1)
        {
            if (!CheckInitialized()) return default;
            
            if (index == -1) index = containedObjects.Count;
            var containsPosition = containedObjectsPositions.ContainsIndex(index);
            if (!containsPosition) RefreshPositionsTo(index);
            
            return containedObjectsPositions[index];
        }

        public void RefreshPattern(GridZonePattern newPattern)
        {
            if (!CheckInitialized()) return;
            
            this.pattern = newPattern;
            containedObjectsPositions.Clear();
            RefreshPositionsTo(containedObjects.LastIndex());
        }

        private void RefreshPositionsTo(int index)
        {
            if (!CheckInitialized()) return;
            
            Vector3 newPosition;
            while (containedObjectsPositions.Count - 1 < index)
            {
                newPosition = pattern.GetPosition(containedObjectsPositions.Count);
                containedObjectsPositions.Add(newPosition);
            }
        }
        
        #endregion
    }
}