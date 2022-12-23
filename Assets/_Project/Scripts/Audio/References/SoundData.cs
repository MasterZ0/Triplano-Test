using UnityEngine;
using TriplanoTest.Shared;
using UnityEngine.Audio;

namespace TriplanoTest.Audio {

    /// <summary>
    /// SoundReference Scriptable Object. You can give position through Transform, Vector2 or nothing at all.
    /// Store a SoundInstance through PlaySound to have more control of it, if needed.
    /// </summary>
    [CreateAssetMenu(menuName = MenuPath.ScriptableObjects + "Sound Data", fileName = "NewSoundData")]
    public class SoundData : ScriptableObject 
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private AudioMixerGroup group;
        [Space]
        [SerializeField] private bool loop;
        [SerializeField] private float volume;
        [SerializeField] private float pitch;

        public AudioClip Clip => clip;
        public AudioMixerGroup Group => group;

        public float Volume => volume;
        public float Pitch => pitch;
        public bool Loop => loop;
    }

}