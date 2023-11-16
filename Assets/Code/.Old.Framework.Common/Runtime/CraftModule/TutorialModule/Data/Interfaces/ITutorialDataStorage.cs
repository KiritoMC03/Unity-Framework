using General.Extensions;
using UnityEngine;

namespace GameKit.TutorialModule
{
    public interface ITutorialDataStorage
    {
        
    }

    public static class TutorialDataStorageExtensions
    {
        public static bool IsValid<T>(this ITutorialDataStorage obj)
        {
            if (obj.LogNotNull()) return false;
            if (obj is T) return true;
            Debug.LogWarning($"Object({obj.GetType()}) is not {typeof(T)} or inheritor.");
            return false;
        }

        public static bool NotValid<T>(this ITutorialDataStorage obj) => !obj.IsValid<T>();
    }
}