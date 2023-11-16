using System.Collections.Generic;
using General.Extensions;
using UnityEngine;

namespace GameKit.General.Extensions
{
    public static class IReadOnlyListExtensions
    {

        /// <summary>
        /// Sets the parent of each Transform.
        /// /</summary>
        /// <remarks> O(n) </remarks>
        public static void SetParentAll<T>(this IReadOnlyList<T> list, Transform parent)
            where T : Transform
        {
            for (int i = 0; i < list.Count; i++) 
                list[i].parent = parent;
        }

        /// <summary>
        /// Detaches each element from its parent and moves to the top of the hierarchy.
        /// /</summary>
        /// <remarks> O(n) </remarks>
        public static void UnparentAll<T>(this IReadOnlyList<T> list)
            where T : Transform
        {
            for (int i = 0; i < list.Count; i++) 
                list[i].parent = null;
        }

        /// <summary>
        /// Executes the SetActive() method on each GameObject.
        /// /</summary>
        /// <remarks> O(n) </remarks>
        public static void SetActiveAll(this IReadOnlyList<GameObject> list, bool state)
        {
            for (int i = 0; i < list.Count; i++) 
                list[i].SetActive(state);
        }

        /// <summary>
        /// Sets activity for each element inherited from MonoBehaviour.
        /// /</summary>
        /// <remarks> O(n) </remarks>
        public static void SetEnabled<T>(this IReadOnlyList<T> list, bool state)
            where T : MonoBehaviour
        {
            for (int i = 0; i < list.Count; i++) 
                list[i].enabled = state;
        }

        /// <summary>
        /// Destroys every non-null element inherited from UnityEngine.Object.
        /// /</summary>
        /// <remarks> O(n) </remarks>
        public static void DestroyAllNotNull<T>(this IReadOnlyList<T> list)
            where T : UnityEngine.Object
        {
            for (int i = 0; i < list.Count; i++) 
                list[i].DestroyNotNull();
        }
    }
}