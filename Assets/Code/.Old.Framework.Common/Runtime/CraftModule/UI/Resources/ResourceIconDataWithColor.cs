using System;
using UnityEngine;

namespace GameKit.General.UI.Resources
{
    [Serializable]
    public class ResourceIconDataWithColor : ResourceIconData
    {
        [field: SerializeField]
        public virtual Color Color { get; protected set; }
    }
}