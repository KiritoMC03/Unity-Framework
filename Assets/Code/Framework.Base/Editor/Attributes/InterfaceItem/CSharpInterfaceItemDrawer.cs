using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using General.Extensions;
using UnityEditor;
using UnityEngine;

namespace General.Editor
{
    [CustomPropertyDrawer(typeof(CSharpInterfaceItem<>))]
    public class CSharpInterfaceItemDrawer : PropertyDrawer
    {
        #region Fields

        private const BindingFlags FieldsFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        private const string NotImplementedText = "! Not Implemented !";
        private bool isInitialized;

        #endregion

        #region Unity lifecycle

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Type targetInterfaceType = FindGenericType(property);
            DrawPopup(position, property, label, GetClasses(targetInterfaceType));
            isInitialized = true;
        }

        private Type[] GetClasses(Type interfaceType)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(item => interfaceType.IsAssignableFrom(item) && !item.IsInterface)
                .ToArray();
        }

        private void DrawPopup(Rect position, SerializedProperty property, GUIContent label, Type[] classesTypes)
        {
            SerializedProperty typeFullNameProperty = property.FindPropertyRelative("typeFullName");
            SerializedProperty assemblyProperty = property.FindPropertyRelative("assembly");

            string currentTypeFullName = typeFullNameProperty.stringValue;
            int selectedIndex = GetSelectedIndexByType(currentTypeFullName, classesTypes);
            GUIContent[] content = CreatePopupContentList(ref selectedIndex, currentTypeFullName, classesTypes);

            selectedIndex = Mathf.Clamp(selectedIndex, 0, classesTypes.Length + 1);
            int newSelectedIndex = EditorGUI.Popup(position, label, selectedIndex, content);
            if (isInitialized && selectedIndex == newSelectedIndex) return;
            typeFullNameProperty.stringValue = classesTypes[newSelectedIndex].FullName;
            assemblyProperty.stringValue = classesTypes[newSelectedIndex].Assembly.GetName().Name;
        }

        private GUIContent[] CreatePopupContentList(ref int selectedIndex, string typeFullName, Type[] classesTypes)
        {
            GUIContent[] list = new GUIContent[Math.Max(1, classesTypes.Length)];
            if (classesTypes.Length == 0)
            {
                list[0] = new GUIContent(NotImplementedText);
                return list;
            }

            int index = 0;
            foreach (Type type in classesTypes)
            {
                if (type.FullName == typeFullName) selectedIndex = index;
                list[index] = new GUIContent(type.Name);
                index++;
            }

            return list;
        }

        private int GetSelectedIndexByType(string typeFullName, Type[] classesTypes)
        {
            for (int i = 0; i < classesTypes.Length; i++)
                if (typeFullName == classesTypes[i].FullName)
                    return i;

            return 0;
        }

        private Type FindGenericType(SerializedProperty property)
        {
            string propertyName = property.name;

            Queue<FieldInfo> fields = new Queue<FieldInfo>();
            FieldInfo[] startedFields = property.serializedObject.targetObject.GetType().GetFields(FieldsFlags);
            fields.EnqueueRange(startedFields);

            while (fields.Count > 0)
            {
                FieldInfo current = fields.Dequeue();
                if (current.Name == propertyName)
                    return current.FieldType.GenericTypeArguments.Length > 0
                        ? current.FieldType.GenericTypeArguments[0]
                        : current.FieldType;
                fields.EnqueueRange(current.FieldType.GetFields(FieldsFlags));
            }

            return default;
        }

        #endregion
    }
}