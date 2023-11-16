
using General.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GameKit.General.Utils
{
    public static class GameKitUtils
    {
        #region Fields

        private static readonly string[] RequiredTMPScripts = new[] {"TMP_Settings"};
        private const string TMPAssemblyName = "TextMeshPro";

        #endregion
        
        #region Methods

#if UNITY_EDITOR
        public static UnityEditor.Compilation.Assembly[] GetAssemblies() => 
            UnityEditor.Compilation.CompilationPipeline.GetAssemblies();
        
        public static ScriptDescription FindScriptInUnityAssemblies(string typeName)
        {
            string rootNamespace = UnityEditor.EditorSettings.projectGenerationRootNamespace;
            return FindScriptInUnityAssemblies(typeName, rootNamespace);
        }
        
        public static ScriptDescription FindScriptInUnityAssemblies(string typeName, string assemblyNameOrRootNamespace)
        {
            UnityEditor.Compilation.Assembly currentAssembly;
            bool? isFoundAssembly;
            string domainAssemblyName;
            
            UnityEditor.Compilation.Assembly[] assemblies = GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                currentAssembly = assemblies[i];
                isFoundAssembly = currentAssembly?.name?.Contains(assemblyNameOrRootNamespace);
                if (isFoundAssembly.IsNull() || isFoundAssembly == false)
                    currentAssembly?.rootNamespace?.Contains(assemblyNameOrRootNamespace);
                if (isFoundAssembly == null || !(bool) isFoundAssembly) continue;
                
                Assembly[] domainAssemblies = System.AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly domainAssembly in domainAssemblies)
                {
                    domainAssemblyName = domainAssembly.GetName().Name;
                    if (domainAssemblyName != currentAssembly.name) continue;
                    Type[] types = domainAssembly.GetTypes();
                    foreach (Type currentType in types)
                        if (currentType.Name == typeName)
                            return new ScriptDescription(currentType, currentAssembly.sourceFiles.First(item => item.Contains($"/{typeName}.cs")));
                }
            }

            return default;
        }

        public static string AssetPathToFullPath(string assetPath)
        {
            DirectoryInfo directory = Directory.GetParent(UnityEngine.Application.dataPath);
            return directory?.ToString().Replace('\\', '/') + "/" + assetPath;
        }

        public static bool ProjectHasTextMeshPro()
        {
            bool hasTMP = RequiredTMPScripts.Length > 0;
            ScriptDescription temp;
            for (int i = 0; i < RequiredTMPScripts.Length; i++)
            {
                temp = FindScriptInUnityAssemblies(RequiredTMPScripts[i], TMPAssemblyName);
                hasTMP = hasTMP && temp != null;
            }

            return hasTMP;
        }

#endif

        #endregion
    }
}