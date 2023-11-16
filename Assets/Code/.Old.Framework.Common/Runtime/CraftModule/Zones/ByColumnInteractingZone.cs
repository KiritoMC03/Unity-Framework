using System.Collections.Generic;
using GameKit.CraftModule.Resource;
using General.Extensions;
using UnityEngine;

namespace GameKit.CraftModule.Zones
{
    public class ByColumnInteractingZone : ResourcesZone, IResourceSender
    {
        #region Fields

        [SerializeField]
        protected float popRadius = 1f;

        protected List<StorageZoneColumnCell<IBaseResource>> cellsListLink;
        protected Transform resourcesAccepterTransform;
        protected bool isPreInitialized;
        protected ByColumnInteractingStorageZoneLogic<IBaseResource> internalLogic;

        #endregion

        #region Methods

        public void PreInit(ref List<StorageZoneColumnCell<IBaseResource>> cellsListLink, Transform resourcesAccepterTransform)
        {
            this.cellsListLink = cellsListLink;
            this.resourcesAccepterTransform = resourcesAccepterTransform;
            isPreInitialized = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Before calling this method, you must call PreInit().
        /// The resources from A will be copied to the current resource spaces.
        /// To set a link to a list of elements (for example, for LocalDB), you need to use the PreInit() method.
        /// ResourcesList can be empty.
        /// </summary>
        /// <param name="buffer">Will be ignored. Use PreInit() and cellsListLink.</param>
        public override void Init(List<IBaseResource> buffer, int capacity)
        {
            if (!isPreInitialized)
            {
                Debug.LogWarning($"Before calling this method, you must call PreInit()!", gameObject);
                return;
            }
            
            internalLogic = new ByColumnInteractingStorageZoneLogic<IBaseResource>(capacity, 
                popRadius,
                defaultPatternPreferences, 
                ref cellsListLink, 
                resourcesAccepterTransform);
            logic = internalLogic;
            ValidateResourcesPositions();
        }

        public void ReplenishRange(List<IBaseResource> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                logic.TryReplenish(list[i]);
            }
        }

        public virtual bool TryPopLastItem(out IBaseResource target) => internalLogic.TryPopLastObject(out target);
        public virtual bool TrySearchNearestNotFluentlyColumn(out Vector3 result, Vector3 receiverPosition) =>
            internalLogic.TrySearchNearestNotFluentlyColumn(out result, receiverPosition);
        
        public override void ValidateResourcesPositions()
        {
            List<StorageZoneColumnCell<IBaseResource>> items = internalLogic.ContainedCells;
            Quaternion rotation = default;
            if (useItemsLocalRotation)
                rotation = defaultPatternPreferences.center.rotation * Quaternion.Euler(itemsLocalRotation);
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].content.IsNull()) continue;
                items[i].content.CacheTransform.position = GetItemPosition(i);
                items[i].content.CacheTransform.rotation = rotation;
            }
        }

        public override Vector3 GetItemPosition()
        {
            bool searchResult = 
                internalLogic.ContainedCells.TryFindFluentlyCellIndex(logic.Capacity, out int index, fliedInResourcesNumber);
            return searchResult ? logic.GetItemPosition(index) : default;
        }

        #endregion
    }
}