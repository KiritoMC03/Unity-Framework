using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Framework.Base.SaveLoad;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Framework.Base.Editor
{
    public sealed class DependencyController
    {
        #region Fields

        private const string DefinitionDefine = "-define:";
        private const string FileName = "csc.rsp";
        private const string Default = "Default";

        private static readonly StringBuilder StringBuilder = new StringBuilder();

        private static DefinesWrapper DefinesWrapper;

        private static Dictionary<string, (List<string> defines, int index)> DefinesDictionary =
            new Dictionary<string, (List<string> defines, int)>();

        private static DependencyController instance;

        #endregion

        #region Properties

        private static string Path =>
            global::System.IO.Path.Combine(Application.dataPath, FileName);

        #endregion

        #region Methods

        public static void AddDefines(string[] defines, string sectionName = null) =>
            AddDefines(defines.ToList(), sectionName);

        public static void AddDefines(List<string> defines, string sectionName = null)
        {
            Init();
            sectionName = string.IsNullOrEmpty(sectionName) ? Default : sectionName;
            if (DefinesDictionary.ContainsKey(sectionName))
                DefinesWrapper.Defines[DefinesDictionary[sectionName].index].defines = defines;
            else
                DefinesWrapper.Defines.Add(new Define(defines, sectionName));
            SaveUpdate();
        }

        public static void AddDefine(string define, string sectionName = null)
        {
            Init();
            if (string.IsNullOrEmpty(define)) return;
            sectionName = string.IsNullOrEmpty(sectionName) ? Default : sectionName;
            if (DefinesDictionary.ContainsKey(sectionName))
            {
                if (!DefinesWrapper.Defines[DefinesDictionary[sectionName].index].defines.Contains(define))
                    DefinesWrapper.Defines[DefinesDictionary[sectionName].index].defines.Add(define);
                else
                    return;
            }
            else
            {
                List<string> NewList = new List<string>();
                NewList.Add(define);
                DefinesWrapper.Defines.Add(new Define(NewList, sectionName));
            }

            SaveUpdate();
        }

        /// <summary>
        /// Determines whether the Dependency Controller contains the specified define.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns>true if the Dependency Controller contains an element with the specified define; otherwise, false.</returns>
        /// <remarks> O(N^2) </remarks>
        public static bool HasDefine(string define)
        {
            bool result = false;
            Init(false);
            if (string.IsNullOrEmpty(define)) return result;
            foreach ((List<string> defines, int index) item in DefinesDictionary.Values)
            {
                result = item.defines.Contains(define);
                if (result) break;
            }

            return result;
        }


        /// <summary>
        /// Determines whether the Dependency Controller contains the specified section name.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns>true if the Dependency Controller contains an element with the specified section name; otherwise, false.</returns>
        /// <remarks> O(1) </remarks>
        public static bool HasSection(string sectionName)
        {
            Init(false);
            return !string.IsNullOrEmpty(sectionName) && DefinesDictionary.ContainsKey(sectionName);
        }

        public static void DeleteDefine(string define)
        {
            Init();
            if (string.IsNullOrEmpty(define)) return;
            foreach ((List<string> defines, int index) item in DefinesDictionary.Values) item.defines.Remove(define);
            SaveUpdate();
        }

        public static void DeleteSection(string sectionName)
        {
            Init();
            if (string.IsNullOrEmpty(sectionName)) return;
            DefinesWrapper.Defines.RemoveAt(DefinesDictionary[sectionName].index);
            DefinesDictionary.Remove(sectionName);
            SaveUpdate();
        }

        [MenuItem("Plugins/General Plugin/Dependency Controller/Update")]
        public static void Update()
        {
            Init();
        }

        private static void Init(bool requestScriptCompilation = true)
        {
            if (SLComponent.Instance.TryLoad(ref DefinesWrapper))
            {
                UpdateInternal(requestScriptCompilation);
            }
            else
            {
                DefinesWrapper = new DefinesWrapper(null);
                DefinesWrapper.Defines.Add(new Define(null, Default));
                SLComponent.Instance.TrySave(ref DefinesWrapper);
                Save();
            }
        }

        private static void UpdateInternal(bool requestScriptCompilation = true)
        {
            DefinesDictionary.Clear();
            for (int index = 0; index < DefinesWrapper.Defines.Count; index++)
            {
                Define item = DefinesWrapper.Defines[index];
                DefinesDictionary.Add(item.sectionName, (item.defines, index));
            }

            CreateConfiguration(DefinesWrapper.Defines);
            Save();
            if (requestScriptCompilation)
                CompilationPipeline.RequestScriptCompilation();
        }

        private static void SaveUpdate()
        {
            SLComponent.Instance.TrySave(ref DefinesWrapper);
            UpdateInternal();
        }

        private static void CreateConfiguration(List<Define> defines)
        {
            StringBuilder.Clear();
            foreach (Define item in defines)
            {
                if (item.defines is null || item.defines.Count <= 0) continue;
                StringBuilder.AppendLine();
                foreach (string define in item.defines) StringBuilder.AppendLine(DefinitionDefine + define);
                StringBuilder.AppendLine();
            }
        }

        private static void Save()
        {
            File.WriteAllText(Path,
                !string.IsNullOrEmpty(StringBuilder.ToString()) ? StringBuilder.ToString() : string.Empty);
        }

        #endregion
    }
}