using System;
using UnityEngine;

namespace Framework.Base.Movement
{
    [Serializable]
    public struct MovementPointData
    {
        public Transform point;
        public float movementDuration;
    }
}