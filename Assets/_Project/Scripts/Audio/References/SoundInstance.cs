using UnityEngine;

namespace TriplanoTest.Audio    
{
    /// <summary>
    /// An instance of the sound. Use SoundData's "PlaySound" method to receive a SoundInstance and keep track of the audio.
    /// This class has all the values related to the currently playing sound.
    /// </summary>
    public class SoundInstance : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        public void SetSound(SoundData soundData)
        {
            gameObject.name = $"[Sound] {soundData.name}";

            audioSource.clip = soundData.Clip;
            audioSource.outputAudioMixerGroup = soundData.Group;

            audioSource.volume = soundData.Volume;
            audioSource.pitch = soundData.Pitch;
            audioSource.loop = soundData.Loop;
        }

        public void Play()
        {
            audioSource.Play();
        }

        public void StopWithFade() => audioSource.Stop(); // TODO
        public void StopImmediate() => audioSource.Stop();
        public void Pause() => audioSource.Pause();
        public void Unpause() => audioSource.UnPause();
        public bool SoundFinished()
        {
            return audioSource.time >= audioSource.clip.length;
        }
    }
}