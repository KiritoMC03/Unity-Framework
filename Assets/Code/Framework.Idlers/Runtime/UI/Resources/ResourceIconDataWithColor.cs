using System;
using UnityEngine;

namespace Framework.Idlers.UI.Resources
{
    [Serializable]
    public class ResourceIconDataWithColor : ResourceIconData
    {
        [field: SerializeField]
        public virtual Color Color { get; protected set; }
    }
}