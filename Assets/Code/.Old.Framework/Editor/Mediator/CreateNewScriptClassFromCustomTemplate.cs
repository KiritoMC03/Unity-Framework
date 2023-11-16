using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace General.Editor
{
    internal class CreateNewScriptClassFromCustomTemplate
    {
        private const string nameScriptTemplate = "ComponentTemplateScript.cs.txt";
        private const string searchPattern = "*.txt";
        private const string defaultScriptName = "NewComponentScript.cs";
        private const string editorPath = "Assets/Create/C# Script AutoMediator";
        

        [MenuItem(editorPath,false, 51)]
        public static void CreateScriptFromTemplate()
        {
            string projectPath = Path.GetDirectoryName(Application.dataPath);
            string pathToScriptTemplate = default;
            foreach (var item in Directory.GetFiles(projectPath, searchPattern, SearchOption.AllDirectories))
            {
                if (item.EndsWith(nameScriptTemplate))
                {
                    pathToScriptTemplate = item;
                    break;
                }
            }

            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(pathToScriptTemplate, defaultScriptName);
        }
    }
}
