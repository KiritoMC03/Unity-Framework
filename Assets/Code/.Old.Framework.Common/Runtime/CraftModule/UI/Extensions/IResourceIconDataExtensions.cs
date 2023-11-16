using System.Collections.Generic;
using GameKit.CraftModule.Resource;
using GameKit.General.UI.Resources;
using UnityEngine;

namespace GameKit.General.UI.Extensions
{
    public struct IconData : IResourceIconData
    {
        public ResourceType Type { get; }
        public Sprite Icon { get; }
        public Color Color { get; }
        public Color SecondColor { get; }
    }
    
    public static class IResourceIconDataExtensions
    {
        public static Sprite FindResourceIconByType(this IReadOnlyList<IResourceIconData> iconsList, ResourceType resourceType)
        {
            for (int i = 0; i < iconsList.Count; i++)
            {
                IResourceIconData current = iconsList[i];
                if (current.Type == resourceType)
                    return current.Icon;
            }

            Debug.LogWarning($"Icon for resource type {resourceType.Value} not found.");
            return default;
        }
        
        public static T FindResourceIconDataByType<T>(this IReadOnlyList<T> iconsList,
            ResourceType resourceType)
            where T : IResourceIconData
        {
            for (int i = 0; i < iconsList.Count; i++)
            {
                T current = iconsList[i];
                if (current.Type == resourceType)
                    return current;
            }

            Debug.LogWarning($"Icon data for resource type {resourceType.Value} not found.");
            return default;
        }
    }
}