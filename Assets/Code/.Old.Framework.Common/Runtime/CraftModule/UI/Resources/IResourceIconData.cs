using GameKit.CraftModule.Resource;
using UnityEngine;

namespace GameKit.General.UI.Resources
{
    public interface IResourceIconData
    {
        public ResourceType Type { get; }
        public Sprite Icon { get; }
    }
}