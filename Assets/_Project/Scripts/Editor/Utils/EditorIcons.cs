using UnityEditor;
using UnityEngine;

namespace TriplanoTest.AppEditor.Design
{
    // See more icons here: https://github.com/halak/unity-editor-icons
    public enum IconType
    {
        None,
        Lamp,
        Box,
        Cog,
        Gamepad,
        VerticalLayoutGroup,
        ParticleSystemForceField,
        AudioMixerController,
        Eye,
        Globo,
        Avatar,
        Body
    }

    public static class EditorIcons
    {
        public static Texture GetTexture(IconType iconType)
        {
            return GetGUIContent(iconType).image;
        }

        public static GUIContent GetGUIContent(IconType iconType)
        {
            string iconPath = iconType switch
            {
                IconType.Lamp => "PointLight Gizmo",
                IconType.Box => "d_Package Manager@2x",
                IconType.Cog => "d_SettingsIcon@2x",
                IconType.Gamepad => "d_UnityEditor.GameView@2x",
                IconType.VerticalLayoutGroup => "d_VerticalLayoutGroup Icon",
                IconType.ParticleSystemForceField => "ParticleSystemForceField Gizmo",
                IconType.AudioMixerController => "AudioMixerController On Icon",
                IconType.Eye => "d_ViewToolOrbit On@2x",
                IconType.Globo => "d_Profiler.GlobalIllumination@2x",
                IconType.Avatar => "d_AvatarSelector@2x",
                IconType.Body => "BodySilhouette",
                _ => string.Empty,
            };

            if (string.IsNullOrEmpty(iconPath))
                return GUIContent.none;

            return EditorGUIUtility.IconContent(iconPath);
        }
    }
}