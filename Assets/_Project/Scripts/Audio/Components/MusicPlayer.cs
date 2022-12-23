using UnityEngine;

namespace TriplanoTest.Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private SoundData soundReference;

        private void Awake()
        {
            AudioManager.SetCurrentMusic(soundReference);
        }

        private void OnDisable()
        {
            AudioManager.SetCurrentMusic(null);
        }
    }
}