using System;
using System.Collections.Generic;
using GameKit.CraftModule.Resource;
using UnityEngine;

namespace GameKit.CraftModule.Buildings
{
    public interface IBuildZone
    {
        event Action AllResourcesReceivedCallback;

        GameObject GameObject { get; }
        Transform Transform { get; }

        void Init(Dictionary<ResourceType, int> resourcesForBuild);
        void SwitchActivity(bool state);
        void Destroy();
    }
}