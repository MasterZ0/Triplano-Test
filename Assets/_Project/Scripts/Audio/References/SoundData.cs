using UnityEngine;
using TriplanoTest.Shared;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace TriplanoTest.Audio
{
    public enum PlayMode
    {
        Sequence,
        Random
    }

    /// <summary>
    /// SoundReference Scriptable Object. You can give position through Transform, Vector2 or nothing at all.
    /// Store a SoundInstance through PlaySound to have more control of it, if needed.
    /// </summary>
    [CreateAssetMenu(menuName = MenuPath.ScriptableObjects + "Sound Data", fileName = "NewSoundData")]
    public class SoundData : ScriptableObject
    {
        [SerializeField] private PlayMode playMode;
        [SerializeField] private List<AudioClip> clips;
        [SerializeField] private AudioMixerGroup group;
        [Space]
        [SerializeField] private bool loop;
        [Range(0f, 1f)]
        [SerializeField] private float volume = 1f;
        [Range(-3f, 3f)]
        [SerializeField] private float pitch = 1f;

        public PlayMode PlayMode => playMode;
        public List<AudioClip> Clips => clips;
        public AudioMixerGroup Group => group;

        public float Volume => volume;
        public float Pitch => pitch;
        public bool Loop => loop;
    }
}