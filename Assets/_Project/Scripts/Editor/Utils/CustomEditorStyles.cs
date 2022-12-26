using UnityEditor;
using UnityEngine;
using System;
using Color = UnityEngine.Color;

namespace TriplanoTest.AppEditor.Design
{
    public enum ButtonStyle
    {
        RoundButtonStyle,
        FlatButtonStyle,
        Toolbar,
        Simple
    }

    public static class CustomEditorStyles
    {
        public static GUIStyle SimpleBtn => new GUIStyle(GUI.skin.button);
        public static GUIStyle ToolbarBtn => EditorStyles.toolbarButton;

        public static GUIStyle ButtonStyle1 = new GUIStyle(EditorStyles.miniButton)
        {
            normal = new GUIStyleState
            {
                background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn.png") as Texture2D,
            },
            border = new RectOffset(4, 4, 12, 12),
            fixedHeight = 22,
            fixedWidth = 60
        };

        public static GUIStyle SquareButtonStyle = new GUIStyle
        {
            normal = new GUIStyleState
            {
                background = Texture2D.grayTexture,
                textColor = Color.white,

            },
            fontSize = 12,
            fontStyle = FontStyle.Normal,
            alignment = TextAnchor.MiddleCenter,
            border = new RectOffset(4, 4, 4, 4),
            padding = new RectOffset(8, 8, 8, 8),
            margin = new RectOffset(4, 4, 4, 4),
            richText = true,
            imagePosition = ImagePosition.ImageLeft,
            fixedHeight = 32, // Keep image size    
            contentOffset = new Vector2(0, -2)
        };

        public static GUIStyle GetStyle(ButtonStyle styleType, TextAnchor alignment = TextAnchor.MiddleCenter)
        {
            GUIStyle style = styleType switch
            {
                ButtonStyle.Simple => SimpleBtn,
                ButtonStyle.Toolbar => ToolbarBtn,
                ButtonStyle.RoundButtonStyle => ButtonStyle1,
                ButtonStyle.FlatButtonStyle => SquareButtonStyle,
                _ => throw new NotImplementedException(),
            };
            style.alignment = alignment;
            return style;
        }
    }
}