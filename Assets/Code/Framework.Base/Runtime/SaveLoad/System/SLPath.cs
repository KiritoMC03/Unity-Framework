using UnityEngine;

namespace Framework.Base.SaveLoad
{
    public class SLPath : MonoBehaviour
    {
        #region Methods

        internal static string GetPath(string fileName)
        {
            string path = default;
#if UNITY_ANDROID && !UNITY_EDITOR || UNITY_IOS && !UNITY_EDITOR
            path = global::System.IO.Path.Combine(Application.persistentDataPath, fileName);
#else
            path = System.IO.Path.Combine(Application.dataPath, fileName);
#endif
            return path;
        }

        #endregion
    }
}