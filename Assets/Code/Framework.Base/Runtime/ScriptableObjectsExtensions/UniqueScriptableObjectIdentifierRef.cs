using System.IO;
using Framework.Base;
using UnityEditor;
using UnityEngine;

namespace Framework.Idlers.ScriptableObject
{
    public class UniqueScriptableObjectIdentifierRef : MonoBehaviour
    {
        [SerializeField] [ReadOnly]
        private UniqueScriptableObjectIdentifier reference;

        private const string ResourcesPathWithAssetsFolder = "Assets" + ResourcesPath;
        private const string ResourcesPath = "/Resources/UniqueScriptableObjectIdentifiers/";
        private const string Extension = ".asset";

        private void Reset()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            if (reference != null)
                return;

            string directory = Application.dataPath + ResourcesPath;
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            UniqueScriptableObjectIdentifier newIdentifier =
                UnityEngine.ScriptableObject.CreateInstance<UniqueScriptableObjectIdentifier>();
            string identifierName = newIdentifier.name = gameObject.name;
            AssetDatabase.CreateAsset(newIdentifier, ResourcesPathWithAssetsFolder + identifierName + Extension);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            reference = newIdentifier;
        }

        public UniqueScriptableObjectIdentifier GetIdentifier() => reference;
    }
}