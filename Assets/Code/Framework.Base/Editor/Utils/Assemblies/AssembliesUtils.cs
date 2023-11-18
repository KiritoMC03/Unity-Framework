using System;
using System.Linq;
using System.Reflection;
using Framework.Base.Extensions;

#if UNITY_EDITOR

namespace Framework.Base.Utils.Editor
{
    public partial class AssembliesUtils
    {
        #region Methods

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
                if (isFoundAssembly == null || !(bool)isFoundAssembly) continue;

                Assembly[] domainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly domainAssembly in domainAssemblies)
                {
                    domainAssemblyName = domainAssembly.GetName().Name;
                    if (domainAssemblyName != currentAssembly.name) continue;
                    Type[] types = domainAssembly.GetTypes();
                    foreach (Type currentType in types)
                        if (currentType.Name == typeName)
                            return new ScriptDescription(currentType,
                                currentAssembly.sourceFiles.First(item => item.Contains($"/{typeName}.cs")));
                }
            }

            return default;
        }

        #endregion
    }
}

#endif