using System;
using System.Text;
using General.Mediator;
#if MEDIATOR_STAKE_TRACE 
using General.Mediator.Runtime;
#endif
using UnityEditor;
using UnityEngine;

namespace General.Editor
{
    public class MediatorInspector : EditorWindow
    {
        private const string GetStackTrace = "Get StackTrace";
        private const string GetTypes = "Get Types";
        private const string Dot = ".";
        private const int Pixels = 10;
        private const int GuiStyleFontSize = 20;
        private const int Height = 30;
        private static readonly StringBuilder stringBuilder = new StringBuilder();
        private static bool isShowTypes = false;
        private static Vector2 vector2 = Vector2.up;
        private static int linesCount = 1;

        private void OnGUI()
        {
            GUILayout.Space(Pixels);

            GUIColor(Color.Lerp(Color.blue, Color.white, 0.9f), Color.black);
            
            var guiStyle = GUIStyleButton(GuiStyleFontSize, FontStyle.Bold);
            var gui = new GUIStyle(GUI.skin.window);
            
            EditorGUILayout.BeginVertical(gui);
            GUIColor(Color.Lerp(Color.green, Color.white, 0.5f), Color.black);
            
#if MEDIATOR_STAKE_TRACE 
            GUIButton(GetStackTrace,guiStyle, Height, GetStackTraces);
#endif
            GUIButton(GetTypes,guiStyle, Height, () => isShowTypes = true);

            GUIColor(Color.white, Color.white);
            vector2 = EditorGUILayout.BeginScrollView(vector2);
            
#if MEDIATOR_STAKE_TRACE
            if (linesCount > 1)
            {
                EditorGUILayout.Space(Pixels);
                EditorGUILayout.TextArea(stringBuilder.ToString(),GUILayout.Height(16 * linesCount));
            }
#endif
            
            if (isShowTypes)
            {
                EditorGUILayout.Space(Pixels);
                var systemTypes = GetTypesInspector(out var lines);

                EditorGUILayout.TextArea(systemTypes.ToString(), GUILayout.Height(18 * lines));
            }
            
            EditorGUILayout.EndScrollView();
                
            EditorGUILayout.EndVertical();
           
            GUIColor(Color.white, Color.black);
            
        }

        private static StringBuilder GetTypesInspector(out int lines)
        {
            var system = (MediatorSystem) MC.Instance;
            var systemTypes = new StringBuilder();
            lines = 0;
            foreach (var item in system.TypeOfInterfaces)
            {
                lines++;
                systemTypes.AppendLine(lines + ".   " + item.Key + "  |  " + item.Value);
            }

            return systemTypes;
        }


        [MenuItem("Plugins/General Plugin/Mediator/Inspector")]
        static void Init()
        {
            var window = (MediatorInspector)GetWindow(typeof(MediatorInspector));
            window.titleContent = new GUIContent("Mediator Inspector");
            window.Show();
        }
        
#if MEDIATOR_STAKE_TRACE
        private void GetStackTraces()
        {
            stringBuilder.Clear();
            linesCount = 0;
            foreach (var items in MediatorStackTrace.stackTraces)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"------ {items.Key} ------");
                linesCount += 3;
                foreach (var item in items.Value)
                {
                    stringBuilder.AppendLine();
                    for (var i = item.FrameCount - 1; i > 0; i--)
                    {
                        var method = item.GetFrame(i).GetMethod();
                        var methodName = method.Name;
                        if (method.ReflectedType is null) continue;
                        var className = method.ReflectedType.Name;
                        if (i == item.FrameCount - 1)
                        {
                            var assembly = item.GetFrame(2).GetMethod().Module.Name;
                            stringBuilder.Append(assembly + " | " + className + Dot + methodName);
                        }
                        else if (i == 1)
                        {
                            stringBuilder.Append(Dot + methodName);
                        }
                        else
                        {
                            stringBuilder.Append(Dot + className + Dot + methodName);
                        }
                    }

                    stringBuilder.Append(" (");
                    var infos = item.GetFrame(1).GetMethod().GetParameters();

                    for (var index = 0; index < infos.Length; index++)
                    {
                        if (index > 0)
                            stringBuilder.Append(" ,");
                        stringBuilder.Append($" {infos[index].Name} -> ");
                        stringBuilder.Append(infos[index].ParameterType);
                    }

                    stringBuilder.Append(" )");

                    linesCount++;
                }

                stringBuilder.AppendLine();
            }

            linesCount++;
        }
#endif
        
        private void GUIButton(string buttonName, GUIStyle guiStyle, int height, Action functionality)
        {
            if (GUILayout.Button(buttonName, guiStyle, GUILayout.MinHeight(height)))
            {
                functionality?.Invoke();
            }
        }

        private GUIStyle GUIStyleButton(int guiStyleFontSize, FontStyle guiStyleFontStyle)
        {
            var guiStyle = new GUIStyle(GUI.skin.button);
            guiStyle.fontSize = guiStyleFontSize;
            guiStyle.fontStyle = guiStyleFontStyle;
            return guiStyle;
        }

        private static void GUIColor(Color color, Color contentColor)
        {
            GUI.color = color;
            GUI.contentColor = contentColor;
        }
    }
}
