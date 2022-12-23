using UnityEngine;

namespace TriplanoTest.Audio
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private SoundData soundData;

        private SoundInstance instance;

        private void OnEnable()
        {
            instance = soundData.PlaySound(transform);
            AudioManager.AddToPauseSoundsList(instance);
        }

        private void OnDisable()
        {
            instance.StopWithFade();
        }
    }
}