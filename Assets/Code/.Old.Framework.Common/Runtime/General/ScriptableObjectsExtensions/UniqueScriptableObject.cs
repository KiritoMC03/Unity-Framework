using System.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameKit.General.ScriptableObject
{
    public abstract class UniqueScriptableObject : UnityEngine.ScriptableObject
    {
#if UNITY_EDITOR
    protected virtual void OnEnable()
    {
        LeaveUnique();
    }

    private async void LeaveUnique()
    {
        await Task.Delay(100);
        var typeName = GetType().Name;
        var assetsGUIDs = AssetDatabase.FindAssets($"t:{typeName}");
        if (assetsGUIDs.Length > 1) LogAlreadyCreated();

        for (var i = 1; i < assetsGUIDs.Length; i++)
            AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(assetsGUIDs[i]));

        AssetDatabase.Refresh();
    }

    private void LogAlreadyCreated()
    {
        Debug.LogWarning($"<b>Unique asset of type {GetType()} already created. All new assets will be deleted.</b>");
    }
#endif
    }
}