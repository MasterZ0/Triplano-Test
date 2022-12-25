using System;
using TriplanoTest.Audio;
using UnityEngine;

namespace TriplanoTest.Player
{
    [Serializable]
    public sealed class PlayerSFX : PlayerControllerComponent
    {
        [SerializeField] private SoundData jumpSFX;
        [SerializeField] private SoundData landingSFX;

        internal void Jump() => Play(jumpSFX);
        internal void Landing() => Play(landingSFX);

        private void Play(SoundData sfx) => sfx.PlaySound(Controller.transform);
    }
}