using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Framework.Base.Editor
{
    public enum FieldType
    {
        SingleType = 0,
        TypeNotFound,
    }

    [CustomPropertyDrawer(typeof(InterfaceCheckerAttribute))]
    public class InterfaceCheckerEditor : PropertyDrawer
    {
        private FieldType fieldType;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            Type[] types = GetTypes();

            if (!(property.objectReferenceValue is null))
            {
                if (TryFindInterface(property.objectReferenceValue.GetType(), types))
                {
                    fieldType = FieldType.SingleType;
                    EditorGUI.ObjectField(position, property, label);
                }
                else
                {
                    ItemNotFound(ref position, ref property, ref label);
                }
            }
            else
            {
                ItemNotFound(ref position, ref property, ref label);
            }

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            switch (fieldType)
            {
                case FieldType.SingleType:
                    return base.GetPropertyHeight(property, label);
                case FieldType.TypeNotFound:
                    return base.GetPropertyHeight(property, label) + 40;
                default:
                    return base.GetPropertyHeight(property, label);
            }
        }

        private bool TryFindInterface(in Type type, in Type[] types)
        {
            bool[] state = new bool[types.Length];
            Type[] interfaces = type.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            for (int j = 0; j < types.Length; j++)
            {
                Type itemType = types[j];
                if (interfaces[i] == itemType) state[j] = true;
            }

            return state.All(s => s == true);
        }

        private Type[] GetTypes()
        {
            Type[] types = default;

            if (attribute is InterfaceCheckerAttribute obj) types = obj.Types;

            return types;
        }

        private void ItemNotFound(ref Rect position, ref SerializedProperty property, ref GUIContent label)
        {
            fieldType = FieldType.TypeNotFound;
            position.yMax -= 40;
            property.objectReferenceValue = null;
            EditorGUI.ObjectField(position, property, label);
            Rect newPosition = position;
            newPosition.y += 21;
            newPosition.yMax += 17;
            EditorGUI.HelpBox(newPosition,
                $"The required interface is not implemented.",
                MessageType.Warning);
        }
    }
}