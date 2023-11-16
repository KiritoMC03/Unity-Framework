﻿using System;
using GameKit.CraftModule.CollisionResolver;
using UnityEngine;

namespace GameKit.CraftModule.Buildings
{
    [Serializable]
    public class BuildingControllerData
    {
        public Vector3 buildZoneOffset;
        public Vector3 zoneRotation;
        public Vector3 buildZoneScale = Vector3.one;
        public bool useCollisionResolver;
        public TransparentCollisionResolver collisionResolver;
    }
}