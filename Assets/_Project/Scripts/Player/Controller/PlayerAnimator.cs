using System;
using UnityEngine;

namespace TriplanoTest.Player
{
    [Serializable]
    public sealed class PlayerAnimator : PlayerControllerComponent
    {
        [Header("Animator")]
        [SerializeField] private Animator animator;

        [Header("States")]
        [SerializeField] private string idle = "Idle";
        [SerializeField] private string walk = "Walk";
        [SerializeField] private string jump = "Jump";
        [SerializeField] private string crouch = "Crouch";

        internal void Idle(string idleOverride)
        {
            if (string.IsNullOrEmpty(idleOverride))
            {
                idleOverride = idle;
            }

            PlayAnimation(idleOverride);
        }

        public void PlayAnimation(string stateName, float transition = 0.25f) => animator.CrossFade(stateName, transition);
    }
}