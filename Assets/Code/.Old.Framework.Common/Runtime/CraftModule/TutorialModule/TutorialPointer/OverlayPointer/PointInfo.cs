using System;
using DG.Tweening;
using UnityEngine;

namespace GameKit.TutorialModule
{
    [Serializable]
    public class PointInfo
    {
        public Vector2 position;
        public float movementDuration;
        public Ease movementEase;
        public ClickMode clickMode;
        public float onceClickDuration;
    }
}