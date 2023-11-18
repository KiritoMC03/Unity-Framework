using Framework.Base.Dependencies.Mediator;
using Framework.Idlers.Resource;
using UnityEngine;

namespace Framework.Idlers.ResourcesHandlers
{
    public interface IResourcesCreator : ISingleComponent
    {
        bool TryGetResource(ResourceType type, out IBaseResource resource);
        IBaseResource GetResource(ResourceType type);
        void DestroyResource(GameObject target);
    }
}