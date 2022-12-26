using UnityEngine;

namespace TriplanoTest.Shared.Design
{
    public interface IHasIcon
    {
        public IconType IconType { get; }
    }
    public interface IHasCustomIcon
    {
        public Texture Icon { get; }
    }

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
}