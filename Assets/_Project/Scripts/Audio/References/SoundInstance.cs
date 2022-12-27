using TriplanoTest.ObjectPooling;
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

        public bool SoundFinished => !audioSource.isPlaying;

        private SoundData soundData;
        private int soundIndex;

        public void SetSound(SoundData soundData)
        {
            gameObject.name = $"[Sound] {soundData.name}";
            this.soundData = soundData;

            audioSource.outputAudioMixerGroup = soundData.Group;
            audioSource.volume = soundData.Volume;
            audioSource.pitch = soundData.Pitch;
            audioSource.loop = soundData.Loop;

            soundIndex = 0;
            if (soundData.PlayMode == PlayMode.Random)
            {
                soundIndex = Random.Range(0, soundData.Clips.Count);
            }

            audioSource.clip = soundData.Clips[soundIndex];
        }

        public void Play() => audioSource.Play();
        public void StopWithFade() => audioSource.Stop(); // TODO
        public void StopImmediate() => audioSource.Stop();
        public void Pause() => audioSource.Pause();
        public void Unpause() => audioSource.UnPause();
        
        private void Update()
        {
            if (soundData.PlayMode == PlayMode.Sequence)
            {
                bool thereAreSounds = soundIndex < soundData.Clips.Count - 1;
                if (thereAreSounds) 
                {
                    if (SoundFinished)
                    {
                        soundIndex++;
                        audioSource.clip = soundData.Clips[soundIndex];
                        audioSource.Play();
                    }
                    return;
                }
            }

            if (SoundFinished)
            {
                this.ReturnToPool();
            }
        }
    }
}