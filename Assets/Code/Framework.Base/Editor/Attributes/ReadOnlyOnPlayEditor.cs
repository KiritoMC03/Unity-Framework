using UnityEditor;
using UnityEngine;

namespace Framework.Base.Editor
{
    [CustomPropertyDrawer(typeof(ReadOnlyOnPlayAttribute))]
    public class ReadOnlyOnPlayEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (EditorApplication.isPlaying || EditorApplication.isPaused)
                GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            EditorGUI.GetPropertyHeight(property, label, true);
    }
}