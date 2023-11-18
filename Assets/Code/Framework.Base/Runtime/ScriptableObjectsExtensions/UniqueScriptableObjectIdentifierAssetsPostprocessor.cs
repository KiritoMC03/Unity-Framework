using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework.Idlers.ScriptableObject
{
    public class UniqueScriptableObjectIdentifierAssetsPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            List<UniqueScriptableObjectIdentifier> registry = new List<UniqueScriptableObjectIdentifier>(100);
            string[] assetsGuids = AssetDatabase.FindAssets($"t:{nameof(UniqueScriptableObjectIdentifier)}");
            CheckAssets(registry, assetsGuids);
        }

        private static void CheckAssets(List<UniqueScriptableObjectIdentifier> registry, IEnumerable<string> assetGuids)
        {
            foreach (string assetGuid in assetGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(assetGuid);
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                if (asset is not UniqueScriptableObjectIdentifier current)
                    continue;

                current.isValid = true;
                foreach (UniqueScriptableObjectIdentifier other in registry)
                    if (other.Equals(current))
                    {
                        current.isValid = false;
                        break;
                    }
                if (current.isValid) 
                    registry.Add(current);
                current.OnValidate();
            }
        }
    }
}