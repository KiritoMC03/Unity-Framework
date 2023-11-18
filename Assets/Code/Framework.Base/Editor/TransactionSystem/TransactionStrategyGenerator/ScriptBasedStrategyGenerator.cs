#if UNITY_EDITOR
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Framework.Base.Utils;
using Framework.Base.Utils.Editor;
using UnityEditor;

namespace Framework.Base.Transactions.Editor
{
    public class ScriptBasedStrategyGenerator
    {
        #region Fields

        private const BindingFlags MethodFlagsForExtractToInterface = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
        private const string Space = " ";
        private const string Tab = "    ";
        private const string OpenedRoundBracket = "(";
        private const string ClosedRoundBracket = ")";
        private const string EndLine = ";";
        private const string VoidParameterIncorrect = "Void";
        private const string VoidParameterCorrect = "void";

        #endregion
        
        #region Methods

        public static void Generate(string sourceScriptName)
        {
            ScriptDescription script = AssembliesUtils.FindScriptInUnityAssemblies(sourceScriptName);
            string fullPathToSourceScript = AssetsUtils.LocalAssetPathToFullPath(script.path);
            string pathToSourceScriptFolder = Path.GetDirectoryName(fullPathToSourceScript) + "/";
            string transactionInterfaceName = $"I{sourceScriptName}";
            string targetNamespace = EditorSettings.projectGenerationRootNamespace;
            string strategyName = $"Create{sourceScriptName}Strategy";

            GenerateStrategy(pathToSourceScriptFolder, targetNamespace, strategyName, transactionInterfaceName);
            GenerateInterface(pathToSourceScriptFolder, targetNamespace, transactionInterfaceName, script.type);
            AddInterfaceToTransaction(script.path, sourceScriptName, transactionInterfaceName);
        }

        private static void GenerateStrategy(string path, string targetNamespace, string strategyName, string transactionInterfaceName)
        {
            string fileText = "using System;\nusing Framework.Base.Transactions;\n\nnamespace " + targetNamespace + "\n{\n    public class " + strategyName + " : TransactionStrategy\n    {\n        #region ITransactionStrategy\n\n        public override Type TransactionType => typeof(" + transactionInterfaceName + ");\n        public override ITransaction CreateInstance()\n        {\n            throw new NotImplementedException();\n        }\n\n        #endregion\n    }\n}";
            string targetPath = path + strategyName + ".cs";
            File.WriteAllText(targetPath, fileText);
        }

        private static void GenerateInterface(string path, string targetNamespace, string interfaceName, Type transactionType)
        {
            string fileTextStart = "using Framework.Base.Transactions;\n\nnamespace " + targetNamespace + "\n{\n    public interface " + interfaceName + " : ITransaction\n    {";
            string fileTextEnd = "\n    }\n}";
            
            StringBuilder appendMethodsBuilder = new StringBuilder();
            appendMethodsBuilder.Append(fileTextStart);
            MethodInfo[] methodsInfo = transactionType.GetMethods(MethodFlagsForExtractToInterface);
            for (int i = 0; i < methodsInfo.Length; i++) 
                CreateMethodFromInfo(appendMethodsBuilder, methodsInfo[i]);
            appendMethodsBuilder.Append(fileTextEnd);

            path = path + interfaceName + ".cs";
            File.WriteAllText(path, appendMethodsBuilder.ToString());
        }

        private static void CreateMethodFromInfo(StringBuilder stringBuilder, MethodInfo methodsInfo)
        {
            stringBuilder.Append("\n");
            stringBuilder.Append(Tab);
            stringBuilder.Append(Tab);
            stringBuilder.Append(methodsInfo.ReturnType.Name == VoidParameterIncorrect ? VoidParameterCorrect : methodsInfo.ReturnType.Name);
            stringBuilder.Append(Space);
            stringBuilder.Append(methodsInfo.Name);
            stringBuilder.Append(OpenedRoundBracket);

            ParameterInfo[] parameters = methodsInfo.GetParameters();
            bool isFirstParameter = true;
            foreach (ParameterInfo parameter in parameters)
            {
                if (!isFirstParameter) stringBuilder.Append(", ");
                stringBuilder.Append(parameter.ParameterType.Name);
                stringBuilder.Append(Space);
                stringBuilder.Append(parameter.Name);
                isFirstParameter = false;
            }
            
            stringBuilder.Append(ClosedRoundBracket);
            stringBuilder.Append(EndLine);
        }

        private static void AddInterfaceToTransaction(string scriptPath, string transactionName, string interfaceName)
        {
            string fileText = File.ReadAllText(scriptPath);
            int index = fileText.IndexOf(transactionName, StringComparison.Ordinal);
            bool hasInheritance = CheckHasInheritance(fileText, index, fileText.Length);
            string prefix = hasInheritance ? " " : " : ";
            string suffix = hasInheritance ? "," : "";
            int startIndex = index + transactionName.Length + (hasInheritance ? 2 : 0);
            fileText = fileText.Insert(startIndex, $"{prefix}{interfaceName}{suffix}");
            File.WriteAllText(scriptPath, fileText);
        }

        private static bool CheckHasInheritance(string text, int startedIndex, int lastIndex)
        {
            for (int i = startedIndex; i < lastIndex; i++)
            {
                char character = text[i];
                if (character == ':') return true;
                if (character == ' ' || character == '\n' || character == '\t') continue;
                if (character == '{') return false;
            }

            return false;
        }

        #endregion
    }
}
#endif