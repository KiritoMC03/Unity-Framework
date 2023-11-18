using UnityEngine;

namespace Framework.Idlers.Resource
{
    public interface IBaseResource
    {
        public ResourceType Type { get; }
        public GameObject CacheGameObject { get; }
        public Transform CacheTransform { get; }
    }
}