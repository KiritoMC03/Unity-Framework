using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Framework.Base.ComponentModel.Editor
{
    public static class AutoContainersGenerator
    {
        private static readonly DirectoryInfo RootDirectory = new DirectoryInfo(Application.dataPath);
        private static readonly string RootDirectoryStr =
            $"{RootDirectory.ToString().Replace('\\', '/')}/Resources/AutoContainers";

        [InitializeOnLoadMethod]
        private static void Init()
        {
            Application.logMessageReceived -= OnLogMessageReceived;
            Application.logMessageReceived += OnLogMessageReceived;
        }
 
        private static void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            if (type is LogType.Error or LogType.Exception) Generate();
        }
        
        [InitializeOnLoadMethod]
        public static void Generate()
        {
            if (!Directory.Exists(RootDirectoryStr)) Directory.CreateDirectory(RootDirectoryStr);
            
            IEnumerable<Type> types = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(type =>
                    type.IsSubclassOf(typeof(Component)) &&
                    type.CustomAttributes.Any(attr => attr.AttributeType == typeof(AutoContainerAttribute)));

            HashSet<string> paths = new HashSet<string>();
            StringBuilder builder = new StringBuilder(1000);
            string @using = GetUsingFor(typeof(ComponentContainer<>));
            bool changed = false;
            foreach (Type type in types)
            {
                builder.Clear();
                (bool generated, string path) = GenerateForComponent(@using, type, builder);
                paths.Add(new FileInfo(path).FullName);
                changed = changed || generated;
            }

            IEnumerable<FileInfo> notHandlerFiles = new DirectoryInfo(RootDirectoryStr)
                .EnumerateFiles("*.cs", SearchOption.AllDirectories)
                .Where(file => !paths.Contains(file.FullName)); 
            foreach (FileInfo file in notHandlerFiles) File.Delete(file.FullName);

            if (changed)
            {
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                CompilationPipeline.RequestScriptCompilation();
            }
        }

        private static (bool generated, string path) GenerateForComponent(
            string usings, 
            Type componentType, 
            StringBuilder builder)
        {
            string typeName = componentType.Name;
            builder.Append(usings);
            string @using = GetUsingFor(componentType);
            if (usings != @using) builder.Append(@using);
            builder.Append("public partial class ")
                .Append(typeName)
                .Append("Container : ComponentContainer<")
                .Append(typeName)
                .Append('>')
                .Append('{')
                .Append('}')
                .Append('\n');
            string path = GetPathForComponent(typeName);
            string text = builder.ToString();
            if (File.Exists(path) && File.ReadAllText(path) == text) 
                return (false, path);
            File.WriteAllText(path, text);
            return (true, path);
        }

        private static string GetUsingFor(Type type) => $"using {type.Namespace};\n";

        private static string GetPathForComponent(string componentTypeName) => 
            $"{RootDirectoryStr}/{componentTypeName}Container.cs";
    }
}