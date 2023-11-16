using System;
using GameKit.CraftModule.Resource;
using UnityEngine;

namespace GameKit.General.UI.Resources
{
    [Serializable]
    public class ResourceIconData : IResourceIconData
    {
        [field: SerializeField]
        public virtual ResourceType Type { get; protected set; }
        
        [field: SerializeField]
        public virtual Sprite Icon { get; protected set; }
    }
}