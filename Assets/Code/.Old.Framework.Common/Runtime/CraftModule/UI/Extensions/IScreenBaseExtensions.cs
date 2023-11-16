using System.Collections.Generic;
using General.Extensions;
using UnityEngine;

namespace GameKit.General.UI.Extensions
{
    public static class IScreenBaseExtensions
    {
        public static ScreenBase ShowScreen<T>(this IReadOnlyList<ScreenBase> list, Transform parent)
            where T : ScreenBase
        {
            ScreenBase prefab = default;
            for (int i = 0; i < list.Count; i++)
            {
                ScreenBase current = list[i];
                if (!(current is T)) continue;
                prefab = current;
                break;
            }

            if (prefab.NotNull())
            {
                T result = UnityEngine.Object.Instantiate(prefab, parent) as T;
                result.name = prefab.name;
                return result;
            }

            Debug.LogWarning($"Screen of type {typeof(T)} not found.");
            return default;
        }

        public static ScreenBase ShowInitedScreen<T>(this IReadOnlyList<ScreenBase> list, Transform parent)
            where T : ScreenBase
        {
            ScreenBase screen = list.ShowScreen<T>(parent);
            if (screen.NotNull()) screen.Init();
            return screen;
        }
    }
}