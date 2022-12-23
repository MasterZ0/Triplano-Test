using UnityEngine;
using TriplanoTest.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using TriplanoTest.ObjectPooling;

namespace TriplanoTest.Audio
{
    public enum SoundGroup
    {
        Master,
        Music,
        SFX,
        Voice
    }

    public enum UISound
    {
        Submit,
        Cancel,
        Select
    }

    /// <summary>
    /// Manage the sound requests and returns a SoundInstance.
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("Audio Manager")]
        [SerializeField] private SoundInstance soundInstancePrefab;

        [Header("UI")]
        [SerializeField] private SoundData submit;
        [SerializeField] private SoundData cancel;
        [SerializeField] private SoundData select;

        private static SoundInstance currentMusic;

        private static readonly List<SoundInstance> pauseSoundsList = new List<SoundInstance>();
        private static readonly List<SoundInstance> stopSoundsList = new List<SoundInstance>();


        #region Public Methodsx
        public static void SetCurrentMusic(SoundData music)
        {
            if (currentMusic != music)
            {
                if (currentMusic)
                {
                    currentMusic.StopWithFade();
                    currentMusic = null;
                }

                if (music)
                    currentMusic = music.PlaySound();
            }
        }

        public static void PlayUISound(UISound soundType)
        {
            SoundData sound = soundType switch
            {
                UISound.Submit => Instance.submit,
                UISound.Cancel => Instance.cancel,
                UISound.Select => Instance.select,
                _ => throw new NotImplementedException(),
            };

            sound.PlaySound();
        }

        public static void AddToAutoStopList(SoundInstance instance) => stopSoundsList.Add(instance);

        public static void StopSounds()
        {
            foreach (SoundInstance instance in stopSoundsList)
            {
                instance.StopWithFade();
            }

            stopSoundsList.Clear();
        }

        public static void AddToPauseSoundsList(SoundInstance instance) => pauseSoundsList.Add(instance);

        public static void PauseSounds()
        {
            foreach (SoundInstance instance in pauseSoundsList.ToList())
            {
                if (instance.SoundFinished())
                {
                    pauseSoundsList.Remove(instance);
                }
                else
                {
                    instance.Pause();
                }
            }
        }

        public static void UnpauseSounds()
        {
            foreach (SoundInstance instance in pauseSoundsList)
            {
                instance.Unpause();
            }
        }
        #endregion

        internal static SoundInstance PlaySound(SoundData soundData, Transform transform)
        {
            SoundInstance soundInstance;
            if (transform)
            {
                soundInstance = ObjectPool.SpawnPooledObject(Instance.soundInstancePrefab, transform.position, transform.rotation, transform);
            }
            else
            {
                soundInstance = ObjectPool.SpawnPooledObject(Instance.soundInstancePrefab);
            }

            return SetupSoundInstance(soundInstance, soundData);
        }

        internal static SoundInstance PlaySound(SoundData soundData, Vector3 position)
        {
            SoundInstance soundInstance = ObjectPool.SpawnPooledObject(Instance.soundInstancePrefab, position);
            return SetupSoundInstance(soundInstance, soundData);
        }

        private static SoundInstance SetupSoundInstance(SoundInstance soundInstance, SoundData soundData)
        {
            soundInstance.SetSound(soundData);
            soundInstance.Play();
            return soundInstance;
        }
    }
}