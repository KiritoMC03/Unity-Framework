using UnityEditor;
using UnityEngine;

namespace General.DebugMenu.Editor
{
    public class DMEditor : MonoBehaviour
    {

        #region Methods

        [MenuItem("Plugins/General Plugin/DM/Create")]
        public static void Create()
        {
            if (FindObjectOfType<DM>() is { }) return;
            foreach (var item in AssetDatabase.FindAssets("DM"))
            {
                var data = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(item), typeof(GameObject)) as GameObject;
                if (data is {} && data.TryGetComponent(out DM dm))
                {
                    Instantiate(data, null);
                    break;
                }
            }
        }
        
        [MenuItem("Plugins/General Plugin/DM/Delete")]
        private static void Delete()
        {
            DestroyImmediate(FindObjectOfType<DM>().gameObject);
        }

        #endregion
    }
}
