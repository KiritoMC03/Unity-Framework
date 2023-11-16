using System;
using System.Collections.Generic;
using System.Reflection;
using General.Extensions;
using UnityEditor;
using UnityEngine;

namespace General.Editor
{
    [CustomPropertyDrawer(typeof(InterfaceItem<>))]
    public class InterfaceItemDrawer : PropertyDrawer
    {
        #region Fields

        private const int InterfaceNotFoundMessageHeight = 20;
        private const string RelativeComponentContainedPropertyName = "component";
        private const BindingFlags FieldsFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        #endregion

        #region Unity lifecycle

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty componentProperty =
                property.FindPropertyRelative(RelativeComponentContainedPropertyName);
            EditorGUI.PropertyField(position, componentProperty, label, true);
            Type targetInterfaceType = FindGenericType(property);
            WriteRequireInterface(ref position, targetInterfaceType);
            if (componentProperty.objectReferenceValue.IsNull())
                return;

            FindInterface(componentProperty, targetInterfaceType);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            base.GetPropertyHeight(property, label) +
            InterfaceNotFoundMessageHeight;

        #endregion

        #region Methods

        private Type FindGenericType(SerializedProperty property)
        {
            string propertyName = property.name;

            Queue<FieldInfo> fields = new Queue<FieldInfo>();
            FieldInfo[] startedFields = property.serializedObject.targetObject.GetType().GetFields(FieldsFlags);
            fields.EnqueueRange(startedFields);

            string[] path = property.propertyPath.Split('.');
            Type currenFieldType = property.serializedObject.targetObject.GetType();
            string currentFieldName = "";
            foreach (string p in path)
            {
                FieldInfo field = currenFieldType.GetField(p, FieldsFlags);
                if (currenFieldType.IsArray)
                {
                    currenFieldType = currenFieldType.GetElementType();
                }
                else if (field != null)
                {
                    currenFieldType = field.FieldType;
                    currentFieldName = field.Name;
                }
            }

            if (currenFieldType != null && currentFieldName == propertyName)
                return currenFieldType.GenericTypeArguments[0];

            return default;
        }

        private void FindInterface(in SerializedProperty componentProperty, in Type targetInterfaceType)
        {
            Component component = default;
            Type currentObjectType = componentProperty.objectReferenceValue.GetType();
            if (TryFindInterface(currentObjectType, targetInterfaceType))
                return;
            else if (componentProperty.objectReferenceValue is GameObject itemGameObject)
                TryFindInterface(itemGameObject, targetInterfaceType, out component);
            else if (componentProperty.objectReferenceValue is Component itemComponent)
                TryFindInterface(itemComponent.gameObject, targetInterfaceType, out component);

            componentProperty.objectReferenceValue = component;
        }

        private bool TryFindInterface(in GameObject obj, in Type targetInterfaceType, out Component component)
        {
            component = default;
            if (obj.IsNull()) return false;
            MonoBehaviour[] components = obj.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour item in components)
            {
                component = item;
                if (TryFindInterface(component.GetType(), targetInterfaceType))
                    return true;
            }

            return false;
        }

        private bool TryFindInterface(in Type type, in Type targetInterfaceType)
        {
            Type[] interfaces = type.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
                if (interfaces[i] == targetInterfaceType)
                    return true;

            return false;
        }

        private static void WriteRequireInterface(ref Rect position, Type interfaceType)
        {
            Rect newPosition = position;
            newPosition.y += 21;
            newPosition.yMax -= 17;
            newPosition.height = InterfaceNotFoundMessageHeight;
            EditorGUI.HelpBox(newPosition,
                $"Require interface {interfaceType.Name}.",
                MessageType.Info);
        }

        #endregion
    }
}