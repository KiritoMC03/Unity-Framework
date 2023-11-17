#if UNITY_EDITOR
using System.IO;
using Framework.Base.Editor;
using Framework.Base.Editor.EnumCreator;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Framework.Base.ObjectPool.Editor
{
    public class PoolerEditorInitializer
    {
        #region Fields

        private const string ObjectPoolerDefine = "OBJECT_POOLER";

        // Paths:
        private static readonly string ResourcesPath = "Assets/Resources";
        private static readonly string ObjectPoolerResourcePath = "Assets/Resources/ObjectPooler";

        private const string ToGeneralAssemblyReferenceText = "{\"reference\": \"Framework.Base.Runtime\" }";

        #endregion

        #region Methods

        public static void Init(string enumMembersAssetName, string enumCreatorConfigAssetName,
            string pooledObjectsInfoAssetName)
        {
            EnumMembersConfig enumMembersConfig;
            EnumCreatorConfig enumCreatorConfig;
            if (!TryCreateScriptable(enumMembersAssetName, enumCreatorConfigAssetName, pooledObjectsInfoAssetName,
                    out enumCreatorConfig, out enumMembersConfig))
            {
                Debug.LogWarning("Creating of Scriptable Objects for ObjectPooler fail.");
                return;
            }

            EnumCreator.Create(enumCreatorConfig, enumMembersConfig,
                ObjectPoolerResourcePath + $"/{enumCreatorConfig.csFileName}.cs");
            CreateAssemblyReference();
            CompilationPipeline.RequestScriptCompilation();
            DependencyController.AddDefine(ObjectPoolerDefine);
        }

        private static bool TryCreateScriptable(string enumMembersAssetName, string enumCreatorConfigAssetName,
            string pooledObjectsInfoAssetName,
            out EnumCreatorConfig enumCreatorConfig, out EnumMembersConfig enumMembersConfig)
        {
            enumMembersConfig = ScriptableObject.CreateInstance<EnumMembersConfig>();
            enumCreatorConfig = ScriptableObject.CreateInstance<EnumCreatorConfig>();
            PooledObjectsInfo pooledObjectsInfo = ScriptableObject.CreateInstance<PooledObjectsInfo>();

            enumCreatorConfig.targetDefine = ObjectPoolerDefine;
            enumCreatorConfig.targetNamespace = "ObjectPool";
            enumCreatorConfig.csFileName = "PooledObjectType";
            enumCreatorConfig.useDefines = true;
            enumCreatorConfig.useNamespace = true;
            enumCreatorConfig.ignoreExistInPaths =
                new[] { "Framework.Base/Runtime/ObjectPooler/Enums/PooledObjectType" };

            if (!AssetDatabase.IsValidFolder(ResourcesPath))
                AssetDatabase.CreateFolder("Assets", "Resources");
            if (!AssetDatabase.IsValidFolder(ObjectPoolerResourcePath))
                AssetDatabase.CreateFolder(ResourcesPath, "ObjectPooler");

            AssetDatabase.CreateAsset(enumMembersConfig, $"{ObjectPoolerResourcePath}/{enumMembersAssetName}.asset");
            AssetDatabase.CreateAsset(enumCreatorConfig,
                $"{ObjectPoolerResourcePath}/{enumCreatorConfigAssetName}.asset");
            AssetDatabase.CreateAsset(pooledObjectsInfo,
                $"{ObjectPoolerResourcePath}/{pooledObjectsInfoAssetName}.asset");
            AssetDatabase.SaveAssets();

            return true;
        }

        private static void CreateAssemblyReference()
        {
            string path = $"{ObjectPoolerResourcePath}\\ToFrameworkBaseAssemblyReference.asmref";
            File.WriteAllText(path, ToGeneralAssemblyReferenceText);
        }

        #endregion
    }
}
#endif