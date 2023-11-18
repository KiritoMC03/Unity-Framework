using UnityEngine;

namespace Framework.Idlers.CollisionResolver
{
    public interface ICollisionResolverTarget
    {
        public Transform Transform { get; }
    }
}