using System.Collections.Generic;
using GameKit.CraftModule.Resource;
using GameKit.CraftModule.ResourceCreator;
using General;
using General.Mediator;
using ObjectPool;
using UnityEngine;

namespace GameKit.CraftModule.ResourcesHandlers
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