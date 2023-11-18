using System;
using System.Collections.Generic;
using Framework.Idlers.Resource;
using UnityEngine;

namespace Framework.Idlers.Buildings
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