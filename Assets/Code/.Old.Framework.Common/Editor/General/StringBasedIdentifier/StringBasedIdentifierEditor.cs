using System;
using System.Linq;
using GameKit.CraftModule.Resource;
using General.Extensions;
using UnityEditor;
using UnityEngine;

namespace GameKit.General.Structures
{
    [CustomPropertyDrawer(typeof(StringBasedIdentifier), true)]
    public class StringBasedIdentifierEditor : PropertyDrawer
    {
        #region Fields

        private StringBasedIdentifiersConfig config;
        private string[] typesList;
        private const string TypesListIsEmptyMessage = "The types list is empty. Please, fill config.";

        #endregion

        #region Unity lifecycle

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            var propertyType = property.boxedValue.GetType();
            LoadConfig(propertyType, out config);
            InitTypesList(config);
            return true;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (config.IsNull())
            {
                var propertyType = property.boxedValue.GetType();
                LoadConfig(propertyType, out config);
                InitTypesList(config);
            }

            if (typesList.Length > 0)
                DrawPopup(position, property, label);
            else
                TypesListIsEmpty(ref position);
        }

        #endregion

        #region Methods
        
        private void LoadConfig(Type propertyType, out StringBasedIdentifiersConfig config)
        {
            config = default;
            string[] assetList = AssetDatabase.FindAssets($"t:{nameof(StringBasedIdentifiersConfig)}");

            bool assetFound = false;
            var targetName = StringBasedIdentifiersConfig.GetAssetFileName(propertyType);
            foreach (var asset in assetList)
            {
                var tempAsset = AssetDatabase.LoadAssetAtPath<StringBasedIdentifiersConfig>(AssetDatabase.GUIDToAssetPath(asset));
                if (tempAsset.name + ".asset" == targetName)
                {
                    config = tempAsset;
                    assetFound = true;
                    break;
                }
            }
            
            if (!assetFound)
            {
                config = UnityEngine.ScriptableObject.CreateInstance<StringBasedIdentifiersConfig>();
                AssetDatabase.CreateAsset(config, $"Assets/Resources/{targetName}");
            }
        }

        private void InitTypesList(StringBasedIdentifiersConfig config)
        {
            typesList = config.typesList.ToArray();
        }

        private void DrawPopup(Rect position, SerializedProperty property, GUIContent label)
        { 
            SerializedProperty valueProperty = property.FindPropertyRelative("value");
            int selectedIndex = GetSelectedIndexByType(valueProperty.stringValue);
            GUIContent[] content = CreatePopupContentList(ref selectedIndex, valueProperty);
                
            selectedIndex = EditorGUI.Popup(position, label, selectedIndex, content);
            valueProperty.stringValue = content[selectedIndex].text;
        }

        private int GetSelectedIndexByType(string typeName)
        {
            for (int i = 0; i < typesList.Length; i++)
            {
                if (typeName == typesList[i]) return i;
            }

            return 0;
        }

        private GUIContent[] CreatePopupContentList(ref int selectedIndex, SerializedProperty valueProperty)
        {
            GUIContent[] list = new GUIContent[typesList.Length];
            int index = 0;
            
            foreach (string type in typesList)
            {
                if (type == valueProperty.stringValue) selectedIndex = index;
                list[index] = new GUIContent(type);
                index++;
            }

            return list;
        }

        private void TypesListIsEmpty(ref Rect position)
        {
            position.height = 20;
            EditorGUI.HelpBox(position, TypesListIsEmptyMessage, MessageType.Warning); 
        }

        #endregion
    }
}
