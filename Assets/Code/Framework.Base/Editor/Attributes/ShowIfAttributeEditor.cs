using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Framework.Base.Editor
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfAttributeEditor : PropertyDrawer
    {
        private bool toggle = default;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (property)
            {
                string toggleName = ((ShowIfAttribute)attribute).Toggle;

                if (fieldInfo.IsStatic) return;

                if (fieldInfo.ReflectedType != null)
                {
                    FieldInfo[] fields =
                        fieldInfo.ReflectedType.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                                                          BindingFlags.Instance);

                    foreach (FieldInfo item in fields)
                        if (toggleName == item.Name)
                        {
                            toggle = (bool)item.GetValue(property.serializedObject.targetObject);
                            break;
                        }
                }

                bool isArray = fieldInfo.FieldType.IsArray;

                if (toggle && !isArray) EditorGUI.PropertyField(position, property, label);

                if (isArray) EditorGUI.PropertyField(position, property, label);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (toggle && !fieldInfo.FieldType.IsArray) return EditorGUI.GetPropertyHeight(property);
            if (fieldInfo.FieldType.IsArray) return EditorGUI.GetPropertyHeight(property);
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }
}