using UnityEditor;
using UnityEngine;
using System;

namespace TriplanoTest.AppEditor.Design
{
    public static class EditorComponents
    {
        public static bool ButtonWithIconAndLabel(string name, IconType iconType)
        {
            GUIStyle style = CustomEditorStyles.GetStyle(ButtonStyle.FlatButtonStyle, TextAnchor.MiddleLeft);
            Texture iconTexture = EditorIcons.GetTexture(iconType);

            if (iconTexture != null) // Custom padding :D
            {
                name = "  " + name;
            }

            return GUILayout.Button(new GUIContent(name, iconTexture), style);
        }

        public static bool DrawButton(string name, ButtonStyle styleType = ButtonStyle.Simple, TextAnchor alignment = TextAnchor.MiddleCenter)
        {
            GUIStyle style = CustomEditorStyles.GetStyle(styleType, alignment);
            return GUILayout.Button(name, style);
        }

        public static bool DrawIconButton(IconType iconType, ButtonStyle styleType = ButtonStyle.Simple, params GUILayoutOption[] options)
        {
            GUIContent icon = EditorIcons.GetGUIContent(iconType);
            GUIStyle style = CustomEditorStyles.GetStyle(styleType);
            return GUILayout.Button(icon, style, options);
        }

        public static void DrawContentWithScroll(ref Vector2 scrollPos, Action contentToDraw)
        {
            EditorGUILayout.BeginVertical();
            {
                // Adds a scrollview to allow for sidebar tweaking
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                {
                    contentToDraw.Invoke();
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
    }
}