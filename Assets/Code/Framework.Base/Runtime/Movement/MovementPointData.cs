using System;
using UnityEngine;

namespace GameKit.General.Movement
{
    [Serializable]
    public struct MovementPointData
    {
        public Transform point;
        public float movementDuration;
    }
}