using System.IO;
using General.CSV;
using UnityEditor;
using UnityEngine;

namespace General.Editor
{
    public class CSVInspector : EditorWindow
    {
        #region Fields

        public const string path = "Assets/CSVConfig.asset";
        private const string ThisDLLNameAlreadyExists = "Same name for DLL already used and exist!!";
        private const string DoYouWantToChangeIt = "Do you want to change it?";
        private const string Yes = "Yes";
        private const string No = "No";
        private const string StartLine = "Start Line";
        private const string EndLine = "End Line";
        private const string StartColumn = "Start Column";
        private const string EndColum = "End Colum";
        private const string MenuItemPath = "Plugins/General Plugin/CSV Inspector";
        private const string GenerateAndLoad = "Generate and Load";
        private const string AssemblyName = "Assembly Name";
        private const string ConfigName = "Config Name";
        private const string PleaseAddAName = "Please add a name!";
        private const string Ok = "Ok";
        private const string PleaseAddATextAsset = "Please add a text asset!";
        private CSVConfig config;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            if (File.Exists(path))
            {
                config = AssetDatabase.LoadAssetAtPath(path, typeof(ScriptableObject)) as CSVConfig;
            }
            else
            {
                config = CreateInstance<CSVConfig>();
                config.body = new CSV.Rect(0);
                config.header = new CSV.Rect(0);
                AssetDatabase.CreateAsset(config, path);
                AssetDatabase.SaveAssets();
            }
        }

        private void OnGUI()
        {
            if (config is null) Awake();
            if (config is null) return;
            int height = 30;
            GUILayout.Space(20);
            GUIStyle gUI = new GUIStyle(GUI.skin.button);
            gUI.fontSize = 20;
            GUILayout.Space(10);

            config.textAsset = EditorGUILayout.ObjectField("Text", config.textAsset, typeof(TextAsset)) as TextAsset;
            config.assemblyName = EditorGUILayout.TextField(AssemblyName, config.assemblyName);
            config.configName = EditorGUILayout.TextField(ConfigName, config.configName);

            Header();
            Body();

            GUILayout.Space(10);
            if (GUILayout.Button(GenerateAndLoad, gUI))
            {
                if (config.textAsset is null)
                {
                    EditorUtility.DisplayDialog("Text asset is cannot be null!",
                        PleaseAddATextAsset, Ok);
                    return;
                }

                if (string.IsNullOrEmpty(config.assemblyName))
                {
                    EditorUtility.DisplayDialog("Assembly name is cannot be empty!",
                        PleaseAddAName, Ok);
                    return;
                }

                if (string.IsNullOrEmpty(config.configName))
                {
                    EditorUtility.DisplayDialog("Config name is cannot be empty!",
                        PleaseAddAName, Ok);
                    return;
                }

                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssetIfDirty(config);
                if (DLLExist(config.assemblyName))
                {
                    if (EditorUtility.DisplayDialog(ThisDLLNameAlreadyExists,
                            DoYouWantToChangeIt, Yes, No))
                        Run();
                }
                else
                {
                    Run();
                }
            }

            GUILayout.Space(10);
        }

        #endregion

        #region Methods

        private void Run() => PipelineGeneration.Start(config);

        private void Body()
        {
            config.bodyStatus = EditorGUILayout.Foldout(config.bodyStatus, "Body");
            if (config.bodyStatus)
            {
                EditorGUI.indentLevel++;
                config.body.StartLine = EditorGUILayout.IntField(StartLine, config.body.StartLine);
                config.body.EndLine = EditorGUILayout.IntField(EndLine, config.body.EndLine);
                config.body.StartColumn = EditorGUILayout.IntField(StartColumn, config.body.StartColumn);
                config.body.EndColum = EditorGUILayout.IntField(EndColum, config.body.EndColum);
                EditorGUI.indentLevel--;
            }
        }

        private void Header()
        {
            config.headerStatus = EditorGUILayout.Foldout(config.headerStatus, "Header");
            if (config.headerStatus)
            {
                EditorGUI.indentLevel++;
                config.header.StartLine = EditorGUILayout.IntField(StartLine, config.header.StartLine);
                config.header.EndLine = EditorGUILayout.IntField(EndLine, config.header.EndLine);
                config.header.StartColumn = EditorGUILayout.IntField(StartColumn, config.header.StartColumn);
                config.header.EndColum = EditorGUILayout.IntField(EndColum, config.header.EndColum);
                EditorGUI.indentLevel--;
            }
        }

        private bool DLLExist(string assemblyName)
        {
            string path = Path.Combine(Application.dataPath, assemblyName + DynamicBuilder.ExtensionDLL)
                .Replace("\\", "/");
            return File.Exists(path);
        }

        [MenuItem(MenuItemPath)]
        private static void Init()
        {
            CSVInspector window = (CSVInspector)GetWindow(typeof(CSVInspector));
            window.titleContent.text = nameof(CSVInspector);
            window.Show();
        }

        #endregion
    }
}