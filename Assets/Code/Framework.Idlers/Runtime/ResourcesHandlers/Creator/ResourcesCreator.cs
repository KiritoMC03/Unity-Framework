using System.Collections.Generic;
using Framework.Base.Collections;
using Framework.Base.Dependencies.Mediator;
using Framework.Base.ObjectPool;
using Framework.Idlers.Resource;
using Framework.Idlers.ResourceCreator;
using UnityEngine;

namespace Framework.Idlers.ResourcesHandlers
{
    public class ResourcesCreator : IResourcesCreator
    {
        #region Fields

        protected SerializedDictionary<ResourceType, PooledObjectType> typesMatcher;
        protected ResourcesCreator<IBaseResource, string> module;
        
        #endregion

        #region Constructors

        public ResourcesCreator(SerializedDictionary<ResourceType, PooledObjectType> typesMatcher)
        {
            SerializedDictionary<string, PooledObjectType> validTypesMatcher = new SerializedDictionary<string, PooledObjectType>();
            foreach (KeyValuePair<ResourceType, PooledObjectType> type in typesMatcher) 
                validTypesMatcher.Add(type.Key.Value, type.Value);
            
            module = new ResourcesCreator<IBaseResource, string>(validTypesMatcher);
            MC.Instance.Add(this, SetMode.Force);
        }

        #endregion

        #region Methods

        public virtual bool TryGetResource(ResourceType type, out IBaseResource resource) => module.TryGetResource(type.Value, out resource);
        public virtual IBaseResource GetResource(ResourceType type) => module.GetResource(type.Value);
        public virtual void DestroyResource(GameObject target) => module.DestroyResource(target);

        #endregion
    }
}