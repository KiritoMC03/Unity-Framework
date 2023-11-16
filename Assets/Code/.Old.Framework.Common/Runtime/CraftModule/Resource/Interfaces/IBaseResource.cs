using UnityEngine;

namespace GameKit.CraftModule.Resource
{
    public interface IBaseResource
    {
        public GameKit.CraftModule.Resource.ResourceType Type { get; }
        public GameObject CacheGameObject { get; }
        public Transform CacheTransform { get; }
    }
}