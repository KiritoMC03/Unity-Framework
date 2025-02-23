﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;

namespace Framework.Idlers.Extensions
{
    public static class DOTweenExtensions
    {
        public static Sequence DOJumpDynamic(
            this Transform target,
            Transform endValueTransform,
            float jumpPower,
            int numJumps,
            float duration,
            bool snapping = false)
        {
            if (numJumps < 1) numJumps = 1;
            Vector3 startedPosition = target.position;
            float startPosY = startedPosition.y;
            float offsetY = -1f;
            bool offsetYSet = false;

            Sequence sequence = DOTween.Sequence();
            Tween yTween = target.CreateJumpYTweener(jumpPower, numJumps, duration, snapping);
            yTween.OnStart<Tween>((TweenCallback)(() => startPosY = target.position.y));

            sequence.Join(yTween)
                .SetTarget<Sequence>((object)target)
                .SetEase<Sequence>(DOTween.defaultEaseType);

            yTween.OnUpdate<Tween>(
                (TweenCallback)(() =>
                {
                    Vector3 endPosition = endValueTransform.position;
                    if (!offsetYSet)
                    {
                        offsetYSet = true;
                        offsetY = sequence.isRelative ? endPosition.y : endPosition.y - startPosY;
                    }

                    target.ProgressPositionDOJumpDynamic(yTween, startedPosition, endPosition, offsetY);
                }));
            return sequence;
        }

        private static Tween CreateJumpYTweener(
            this Transform target,
            float jumpPower,
            int numJumps,
            float duration,
            bool snapping = false)
        {
            return (Tween)DOTween
                .To(
                    (DOGetter<Vector3>)(() => target.position),
                    (DOSetter<Vector3>)(x => target.position = x),
                    new Vector3(0.0f, jumpPower, 0.0f),
                    duration / (float)(numJumps * 2))
                .SetOptions(AxisConstraint.Y, snapping)
                .SetEase<Tweener>(Ease.OutQuad)
                .SetRelative<Tweener>()
                .SetLoops<Tweener>(numJumps * 2, LoopType.Yoyo);
        }

        private static void ProgressPositionDOJumpDynamic(
            this Transform target,
            Tween yTween,
            Vector3 startedPosition,
            Vector3 endPosition,
            float offsetY)
        {
            Vector3 position = target.position;
            float percentage = yTween.ElapsedPercentage(true);
            position.x = startedPosition.x + (endPosition.x - startedPosition.x) * percentage;
            position.z = startedPosition.z + (endPosition.z - startedPosition.z) * percentage;
            position.y += DOVirtual.EasedValue(0.0f, offsetY, percentage, Ease.OutQuad);
            target.position = position;
        }

        /// <param name="points">Will be enumerate twice</param>
        public static Sequence DoMoveByPoints(
            this Transform target,
            IEnumerable<Transform> points,
            float totalDuration,
            Ease globalEase = Ease.Linear,
            Ease stepsEase = Ease.Linear)
        {
            return DoMoveByPoints(target, points.Select(t => t.position), totalDuration, globalEase, stepsEase);
        }

        /// <param name="points">Will be enumerate twice</param>
        public static Sequence DoMoveByPoints(
            this Transform target,
            IEnumerable<Vector3> points,
            float totalDuration,
            Ease globalEase = Ease.Linear,
            Ease stepsEase = Ease.Linear)
        {
            float totalDistance = 0f;
            Vector3 prevPoint = target.position;
            foreach (Vector3 point in points)
            {
                totalDistance += Vector3.Distance(prevPoint, point);
                prevPoint = point;
            }

            Sequence sequence = DOTween.Sequence()
                .SetEase(globalEase)
                .SetLink(target.gameObject);
            foreach (Vector3 point in points)
            {
                float distancePercent = Vector3.Distance(target.position, point) / totalDistance;
                float duration = totalDuration * distancePercent;
                sequence.Append(target.DOMove(point, duration).SetEase(stepsEase));
            }

            return sequence;
        }
    }
}