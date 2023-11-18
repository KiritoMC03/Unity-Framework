using System.Collections.Generic;
using Framework.Base.Extensions;
using Framework.Idlers.Resource;

namespace Framework.Idlers.ResourceTransferHelper
{
    public class FindResourceStrategies
    {
        #region Fields
        
        protected IList<IBaseResource> resourcesList;

        #endregion

        #region Constructors

        public FindResourceStrategies(IList<IBaseResource> resourcesList)
        {
            this.resourcesList = resourcesList;
        }

        #endregion

        #region Methods

        public bool TryFindIndex(FindResourceStrategyType findType, ICollection<ResourceType> types, out int index)
        {
            switch (findType)
            {
                case FindResourceStrategyType.Forward:
                    if (TryForwardFindIndex(types, out index)) return true;
                    break;
                case FindResourceStrategyType.Backward:
                    if (TryBackwardFindIndex(types, out index)) return true;
                    break;
                default:
                    index = 0;
                    break;
            }

            return false;
        }

        public bool TryForwardFindIndex(ICollection<ResourceType> types, out int index)
        {
            index = -1;
            for (var i = 0; i < resourcesList.Count; i++)
            {
                var currentResource = resourcesList[i];
                if (currentResource.IsNull()) continue;
                if (!types.Contains(currentResource.Type)) continue;
                index = i;
                return true;
            }

            return false;
        }

        public bool TryBackwardFindIndex(ICollection<ResourceType> types, out int index)
        {
            index = -1;
            for (var i = resourcesList.Count - 1; i >= 0; i--)
            {
                var currentResource = resourcesList[i];
                if (!types.Contains(currentResource.Type)) continue;
                index = i;
                return true;
            }

            return false;
        }

        #endregion
    }

    public enum FindResourceStrategyType
    {
        Backward = -1,
        Forward = 1,
    }
}