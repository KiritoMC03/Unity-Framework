using System;
using Framework.Idlers.Resource;
using UnityEngine;

namespace Framework.Idlers.UI.Resources
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