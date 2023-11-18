using Framework.Base.Collections;
using Framework.Base.ObjectPool;
using Framework.Idlers.Resource;
using Framework.Idlers.ResourcesHandlers;
using UnityEngine;

namespace Framework.Idlers.Fabrics
{
    public class CommonResourcesFabric : IResourcesCreator
    {
        protected ResourcesCreator resourcesCreator;

        public CommonResourcesFabric(SerializedDictionary<ResourceType, PooledObjectType> typesMatcher)
        {
            resourcesCreator = new ResourcesCreator(typesMatcher);
        }
        
        public virtual bool TryGetResource(ResourceType type, out IBaseResource resource) => 
            resourcesCreator.TryGetResource(type, out resource);
        public virtual IBaseResource GetResource(ResourceType type) => resourcesCreator.GetResource(type);
        public virtual void DestroyResource(GameObject target) => resourcesCreator.DestroyResource(target);
    }
}