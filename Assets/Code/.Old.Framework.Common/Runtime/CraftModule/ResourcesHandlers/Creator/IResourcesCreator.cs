using GameKit.CraftModule.Resource;
using General.Mediator;
using UnityEngine;

namespace GameKit.CraftModule.ResourcesHandlers
{
    public interface IResourcesCreator : ISingleComponent
    {
        bool TryGetResource(ResourceType type, out IBaseResource resource);
        IBaseResource GetResource(ResourceType type);
        void DestroyResource(GameObject target);
    }
}