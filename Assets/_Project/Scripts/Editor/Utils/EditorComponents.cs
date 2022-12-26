using UnityEditor;
using UnityEngine;
using System;
using Color = UnityEngine.Color;

namespace TriplanoTest.AppEditor
{
    public enum ButtonStyle
    {
        RoundButtonStyle,
        FlatButtonStyle,
        Toolbar,
        Simple
    }

    // See more icons here: https://github.com/halak/unity-editor-icons
    public enum IconType
    {
        Lamp
    }

    public static class EditorComponents
    {
        public static GUIStyle ButtonStyle1
        {
            get
            {
                GUIStyle buttonStyle1 = new GUIStyle(EditorStyles.miniButton);
                buttonStyle1.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn.png") as Texture2D;
                buttonStyle1.border = new RectOffset(4, 4, 12, 12);
                buttonStyle1.fixedHeight = 22;
                buttonStyle1.fixedWidth = 60;
                return buttonStyle1;
            }
        }

        public static GUIStyle SquareButtonStyle = new GUIStyle
        {
            normal = new GUIStyleState
            {
                background = Texture2D.grayTexture,
                textColor = Color.white
            },
            fontSize = 12,
            fontStyle = FontStyle.Normal,
            alignment = TextAnchor.MiddleCenter,
            border = new RectOffset(4, 4, 4, 4),
            padding = new RectOffset(8, 8, 8, 8),
            margin = new RectOffset(4, 4, 4, 4),
            richText = true,
            imagePosition = ImagePosition.ImageAbove,
            contentOffset = new Vector2(0, -2)
        };

        public static GUIStyle SimpleBtn => new GUIStyle(GUI.skin.button);

        public static bool DrawButton(string name, ButtonStyle styleType = ButtonStyle.Simple, TextAnchor alignment = TextAnchor.MiddleCenter)
        {
            return GUILayout.Button(name, GetStyle(styleType, alignment));
        }

        public static GUIStyle GetStyle(ButtonStyle styleType, TextAnchor alignment = TextAnchor.MiddleCenter)
        {
            GUIStyle style = styleType switch
            {
                ButtonStyle.RoundButtonStyle => ButtonStyle1,
                ButtonStyle.FlatButtonStyle => SquareButtonStyle,
                ButtonStyle.Toolbar => EditorStyles.toolbarButton,
                ButtonStyle.Simple => SimpleBtn,
                _ => throw new NotImplementedException(),
            };
            style.alignment = alignment;
            return style;
        }


        public static bool DrawIconButton(IconType iconType, ButtonStyle styleType = ButtonStyle.Simple, params GUILayoutOption[] options)
        {
            string iconPath = iconType switch
            {
                IconType.Lamp => "PointLight Gizmo",
                _ => throw new NotImplementedException(),
            };
            GUIContent icon = EditorGUIUtility.IconContent(iconPath);

            return GUILayout.Button(icon, GetStyle(styleType), options);
        }

        //public static bool DrawFoldout(bool value)
        //{
        //    string iconPath = iconType switch
        //    {
        //        IconType.Lamp => "PointLight Gizmo",
        //        IconType.Lamp => "PointLight Gizmo",
        //        _ => throw new NotImplementedException(),
        //    };
        //    GUIContent icon = EditorGUIUtility.IconContent(iconPath);
        //    // d_icon dropdown@2x
        //    if (GUILayout.Button(icon, GetStyle(styleType), options)
        //    {
        //        return !value;
        //    }
        //    return value;
        //}

        public static void DrawContentWithScroll(ref Vector2 scrollPos, Action contentToDraw)
        {
            EditorGUILayout.BeginVertical(/*EditorStyles.helpBox*/);
            {
                // Adiciona um scrollview para permitir o ajuste da barra lateral
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