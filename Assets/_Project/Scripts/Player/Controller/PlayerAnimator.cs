using System;
using System.Collections;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace TriplanoTest.Player
{
    public enum AnimatorState
    {
        /// <summary> Loop or finished animation </summary>
        Standard,
        /// <summary> New animation to play </summary>
        NewRequest,
        /// <summary> Waiting animator start to play </summary>
        LoadingRequest,
        /// <summary> Animator is running the request </summary>
        PlayingRequest
    }

    [Serializable]
    public sealed class PlayerAnimator : PlayerControllerComponent
    {
        [Header("Animator")]
        [SerializeField] private Animator animator;

        [Header("States")]
        [SerializeField] private string idleState = "Idle";
        [SerializeField] private string walkState = "Walk";
        [SerializeField] private string jumpState = "Jump";
        [SerializeField] private string fallingState = "Falling";
        [SerializeField] private string crouchState = "Crouch";

        private string idleOverride;

        internal void Idle()
        {
            if (string.IsNullOrEmpty(idleOverride))
            {
                idleOverride = idleState;
            }

            Play(idleOverride);
            idleOverride = string.Empty;
        }
        internal void OverrideIdleAsLanding() => idleOverride = string.Empty;

        internal void Falling() => Play(fallingState);
        internal void Jump() => Play(jumpState);
        internal void Walk() => Play(walkState);
        internal void Crounch() => Play(crouchState);

        private void Play(string stateName, float transition = 0.25f, int layerIndex = 0)
        {
            AnimatorStateInfo current = animator.GetCurrentAnimatorStateInfo(layerIndex);
            animator.CrossFadeInFixedTime(stateName, transition, layerIndex, current.normalizedTime);
        }

    }
}