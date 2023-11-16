using UnityEngine;

namespace GameKit.CraftModule.CollisionResolver
{
    public interface ICollisionResolverTarget
    {
        public Transform Transform { get; }
    }
}