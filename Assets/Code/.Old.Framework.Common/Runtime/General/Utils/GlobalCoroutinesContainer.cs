using System.Collections;
using General.Extensions;
using UnityEngine;

namespace GameKit.General.Utils
{
    public static class GlobalCoroutinesContainer
    {
        #region Fields

        private static GlobalCoroutinesContainerInternal instance;

        #endregion
        
        #region Properties

        private static GlobalCoroutinesContainerInternal Instance
        {
            get
            {
                if (instance.IsNull())
                    instance = new GameObject(nameof(GlobalCoroutinesContainer)).AddComponent<GlobalCoroutinesContainerInternal>();
                return instance;
            }
        }

        #endregion

        #region Methods

        public static Coroutine StartCoroutine(IEnumerator routine) => Instance.StartCoroutine(routine);
        public static void StopCoroutine(Coroutine routine) => Instance.StopCoroutine(routine);
        public static void StopAllCoroutines() => Instance.StopAllCoroutines();
        public static void MakeDontDestroyOnLoad() => UnityEngine.Object.DontDestroyOnLoad(Instance);
        public static void Destroy() => UnityEngine.Object.Destroy(Instance.gameObject);

        #endregion
    }
    
    internal class GlobalCoroutinesContainerInternal : MonoBehaviour {}
}