using Framework.Idlers.Resource;
using UnityEngine;

namespace Framework.Idlers.UI.Resources
{
    public interface IResourceIconData
    {
        public ResourceType Type { get; }
        public Sprite Icon { get; }
    }
}