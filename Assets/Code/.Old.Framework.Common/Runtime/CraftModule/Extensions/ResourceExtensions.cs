using UnityEngine;
using DG.Tweening;
using GameKit.CraftModule.Resource;

namespace GameKit.General.Extensions
{
    public static class IResourceExtensions
    {
        public static Sequence JumpTo(this IBaseResource resource, Vector3 endValue, float duration) =>
            resource.CacheTransform.DOJump(endValue, 1f, 1, duration);

        public static Sequence DynamicJumpTo(this IBaseResource resource, Transform endValueTransform, float duration) =>
            resource.CacheTransform.DOJumpDynamic(endValueTransform, 1f, 1, duration);
    }
}