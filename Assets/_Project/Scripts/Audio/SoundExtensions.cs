using UnityEngine;

namespace TriplanoTest.Audio
{
    public static class SoundExtensions
    {
        public static SoundInstance PlaySound(this SoundData soundData, Transform transform = null)
        {
            return AudioManager.PlaySound(soundData, transform);
        }

        public static SoundInstance PlaySound(this SoundData soundData, Vector3 position)
        {
            return AudioManager.PlaySound(soundData, position);
        }
    }
}