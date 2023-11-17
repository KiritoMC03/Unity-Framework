using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Framework.Base.CSV;
using Framework.Base.SaveLoad;
using UnityEditor;
using UnityEngine;

namespace Framework.Base.Editor
{
    public class PipelineGeneration
    {
        #region Nested Types

        private enum CSVStatus
        {
            GenerationDLL = 1,
            GenerationScriptableObject = 2,
            LoadData = 3,
        }

        [Data(nameof(CSVData), SaveLoadType.PlayerPrefs)]
        private struct CSVData
        {
            public CSVStatus Status;
        }

        #endregion

        #region Fields

        private static CSVData saveData = new CSVData();

        #endregion

        #region Methods

        [InitializeOnLoadMethod]
        private static void Init()
        {
            if (SLComponent.Instance.TryLoad(ref saveData))
            {
                Debug.Log(saveData.Status);
                switch (saveData.Status)
                {
                    case CSVStatus.GenerationDLL:
                        GenerationScriptableObject();
                        break;
                    case CSVStatus.GenerationScriptableObject:
                        StartInternal(GetConfig(), out Data data);
                        LoadData(data);
                        break;
                    case CSVStatus.LoadData:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static CSVConfig GetConfig()
        {
            CSVConfig config = null;
            if (File.Exists(CSVInspector.path))
                config = AssetDatabase.LoadAssetAtPath(CSVInspector.path, typeof(ScriptableObject)) as CSVConfig;

            return config;
        }

        public static void Start(CSVConfig config)
        {
            CSVConvertor csvConvertor = StartInternal(config, out Data data);
            AssemblyData assemblyData = csvConvertor.GetTypes(data);
            GenerationDLL(assemblyData, config.assemblyName);
        }

        private static CSVConvertor StartInternal(CSVConfig config, out Data data)
        {
            CSVConvertor CsvConvertor = new CSVConvertor();
            List<string> listHeader = CsvConvertor.GetHeader(config.textAsset, config.header);
            List<List<string>> listData = CsvConvertor.GetData(config.textAsset, config.body);
            data = new Data(listData, listHeader);
            return CsvConvertor;
        }

        private static void GenerationScriptableObject()
        {
            CSVConfig config = AssetDatabase.LoadAssetAtPath(CSVInspector.path, typeof(ScriptableObject)) as CSVConfig;
            ScriptableObject example = ScriptableObject.CreateInstance($"{config.assemblyName}.Wrapper");
            if (example is null)
            {
                Debug.Log(example is null);
                return;
            }

            // path has to start at "Assets"
            string path = $"Assets/{config.configName}.asset";
            Debug.Log(path);
            AssetDatabase.CreateAsset(example, path);
            EditorUtility.SetDirty(example);
            AssetDatabase.SaveAssetIfDirty(example);
            SaveStatus(CSVStatus.GenerationScriptableObject);
            Init();
        }

        private static async void LoadData(Data data)
        {
            await Task.Yield();
            CSVConfig config = AssetDatabase.LoadAssetAtPath(CSVInspector.path, typeof(ScriptableObject)) as CSVConfig;
            string path = $"Assets/{config.configName}.asset";
            ScriptableObject obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
            int indexGeneral = 1;
            foreach (FieldInfo item in obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (FieldInfo field in item.FieldType.GetFields(BindingFlags.Public | BindingFlags.Instance))
                    for (int indexLine = 0; indexLine < data.GetData.Count; indexLine++)
                        if (data.GetData[indexLine][0].StartsWith(field.Name))
                        {
                            Type type = field.FieldType;
                            if (field.FieldType.IsArray)
                            {
                                if (type == typeof(int[]))
                                {
                                    List<int> values = new List<int>();
                                    while (data.GetData[indexLine][0].StartsWith(field.Name))
                                    {
                                        if (int.TryParse(data.GetData[indexLine][indexGeneral], out int num))
                                            values.Add(num);
                                        indexLine++;
                                        if (!(indexLine < data.GetData.Count)) break;
                                    }

                                    field.SetValue(item.GetValue(obj), values.ToArray());
                                    continue;
                                }

                                if (type == typeof(float[]))
                                {
                                    List<float> values = new List<float>();
                                    while (data.GetData[indexLine][0].StartsWith(field.Name))
                                    {
                                        if (float.TryParse(data.GetData[indexLine][indexGeneral], out float num))
                                            values.Add(num);
                                        indexLine++;
                                        if (!(indexLine < data.GetData.Count)) break;
                                    }

                                    field.SetValue(item.GetValue(obj), values.ToArray());
                                    continue;
                                }

                                if (type == typeof(bool[]))
                                {
                                    List<bool> values = new List<bool>();
                                    while (data.GetData[indexLine][0].StartsWith(field.Name))
                                    {
                                        if (bool.TryParse(data.GetData[indexLine][indexGeneral], out bool boolean))
                                            values.Add(boolean);
                                        indexLine++;
                                        if (!(indexLine < data.GetData.Count)) break;
                                    }

                                    field.SetValue(item.GetValue(obj), values.ToArray());
                                    continue;
                                }

                                if (type == typeof(string[]))
                                {
                                    List<string> values = new List<string>();
                                    while (data.GetData[indexLine][0].StartsWith(field.Name))
                                    {
                                        values.Add(data.GetData[indexLine][indexGeneral]);
                                        indexLine++;
                                        if (!(indexLine < data.GetData.Count)) break;
                                    }

                                    field.SetValue(item.GetValue(obj), values.ToArray());
                                }
                            }
                            else
                            {
                                if (type == typeof(int))
                                    if (int.TryParse(data.GetData[indexLine][indexGeneral], out int num))
                                        field.SetValue(item.GetValue(obj), num);
                                if (type == typeof(float))
                                    if (float.TryParse(data.GetData[indexLine][indexGeneral], out float numf))
                                        field.SetValue(item.GetValue(obj), numf);
                                if (type == typeof(bool))
                                    if (bool.TryParse(data.GetData[indexLine][indexGeneral], out bool boolean))
                                        field.SetValue(item.GetValue(obj), boolean);
                                if (type == typeof(string))
                                    field.SetValue(item.GetValue(obj), data.GetData[indexLine][indexGeneral]);
                            }
                        }

                indexGeneral++;
            }

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = obj;
            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssetIfDirty(obj);

            SaveStatus(CSVStatus.LoadData);
        }

        private static void GenerationDLL(AssemblyData assemblyData, string assemblyName)
        {
            DynamicBuilder bulider = new DynamicBuilder(assemblyName);
            DynamicBuilderType builderType = bulider.CreateType("Data");
            foreach (DataType item in assemblyData.StructureDataTypes)
                builderType.CreateField(item.Type, item.Name);
            Type type = builderType.CreateType();
            DynamicBuilderType builderType1 = bulider.CreateType("Wrapper", typeof(ScriptableObject));
            foreach (string item in assemblyData.WrapperDataNames)
                builderType1.CreateField(type, item);
            bulider.Save();
            DirectoryInfo directoryInfo = Directory.GetParent(Application.dataPath);
            string currentPath = Path.Combine(directoryInfo.FullName, bulider.DynamicAssemblyDll);
            string finalPath = Path.Combine(Application.dataPath, bulider.DynamicAssemblyDll);
            currentPath = currentPath.Replace("\\", "/");
            finalPath = finalPath.Replace("\\", "/");
            Debug.Log(currentPath);
            if (File.Exists(finalPath)) File.Delete(finalPath);
            if (File.Exists(currentPath)) File.Move(currentPath, finalPath);
            SaveStatus(CSVStatus.GenerationDLL);
            AssetDatabase.Refresh();
        }

        private static void SaveStatus(CSVStatus status)
        {
            saveData.Status = status;
            SLComponent.Instance.TrySave(ref saveData);
        }

        #endregion
    }
}