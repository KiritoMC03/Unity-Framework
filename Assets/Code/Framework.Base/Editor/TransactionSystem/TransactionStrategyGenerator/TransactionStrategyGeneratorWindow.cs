#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Framework.Base.Transactions.Editor
{
    public class TransactionStrategyGeneratorWindow : EditorWindow
    {
        #region Fields

        private string sourceScriptName = "ExampleScriptName";
        
        // DrawScriptBasedStrategyGenerator
        private const int LabelSpaceHeight = 12;
        private const string ScriptBasedStrategyGeneratorLabel = "Source script name";
        private const int ScriptBasedStrategyGeneratorLabelWidth = 300;
        
        private const string ScriptBasedStrategyGeneratorButtonText = "Generate strategy";
        private const int ScriptBasedStrategyGeneratorButtonWidth = 120;
        
        #endregion
        
        #region Unity lifecycle
        
        private void OnGUI()
        {
            DrawScriptBasedStrategyGenerator();
        }

        #endregion

        #region Menu Item Methods
        
        [MenuItem("Framework/Base/Transaction System/Transaction Strategy Generator")]
        private static void ShowWindow()
        {
            TransactionStrategyGeneratorWindow window = GetWindow<TransactionStrategyGeneratorWindow>();
            window.titleContent = new GUIContent("Transaction Strategy Generator");
            window.Show();
        }

        #endregion

        #region Methods
        
        private void DrawScriptBasedStrategyGenerator()
        {
            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.Space(LabelSpaceHeight);
            EditorGUILayout.PrefixLabel(ScriptBasedStrategyGeneratorLabel);
            sourceScriptName = EditorGUILayout.TextField(sourceScriptName, GUILayout.Width(ScriptBasedStrategyGeneratorLabelWidth));
            if (GUILayout.Button(ScriptBasedStrategyGeneratorButtonText, GUILayout.Width(ScriptBasedStrategyGeneratorButtonWidth))) 
                TransactionStrategyGenerator.ScriptBasedStrategyGenerate(sourceScriptName);

            EditorGUILayout.EndVertical();
        }

        #endregion
    }
}
#endif