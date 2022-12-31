using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace TriplanoTest.Player
{
    [Serializable]
    public sealed class PlayerAnimator : PlayerControllerComponent
    {
        [Header("Player Animator")]
        [SerializeField] private SkinnedMeshRenderer[] skinnedMeshes;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material invisibleMaterial;
        [SerializeField] private Animator animator;
        [SerializeField] private Rig boxRig;

        [Header("States")]
        [SerializeField] private string idleState = "Idle";
        [SerializeField] private string walkState = "Walk";
        [SerializeField] private string runState = "Run";
        [SerializeField] private string jumpState = "Jump";
        [SerializeField] private string fallingState = "Falling";
        [SerializeField] private string crouchState = "Crouch";
        [SerializeField] private string crouchWalkState = "Crouch_Walk";

        [Header("Parameters")]
        [SerializeField] private string velocityXParameter = "VelocityX";
        [SerializeField] private string velocityZParameter = "VelocityZ";

        private Coroutine moveHandsCoroutine;
        private string idleOverride;

        private float velocityX;
        private float velocityZ;
        private float maxVelocityScale;

        internal void Update()
        {
            // Convert velocity
            Vector3 worldVelocity = Controller.Physics.Velocity;
            Vector3 localVelocity = Controller.Physics.Transform.InverseTransformDirection(worldVelocity);

            // Avoid division by 0
            float maxScale = maxVelocityScale == 0 ? 1 : maxVelocityScale; 
            Vector3 velocityScale = localVelocity / maxScale;

            // Apply smooth
            velocityX = Mathf.MoveTowards(velocityX, velocityScale.x, 1f / Data.AnimationBlendDamp * Time.fixedDeltaTime);
            velocityZ = Mathf.MoveTowards(velocityZ, velocityScale.z, 1f / Data.AnimationBlendDamp * Time.fixedDeltaTime);

            // Round value
            float x = Mathf.Round(velocityX * 10f) / 10f;
            float z = Mathf.Round(velocityZ * 10f) / 10f;

            SetFloat(velocityXParameter, x);
            SetFloat(velocityZParameter, z);
        }

        internal void SetMoveSpeedScale(float scale) => maxVelocityScale = scale;

        internal void SetHoldBoxWeight(float value)
        {
            if (moveHandsCoroutine != null)
            {
                Controller.StopCoroutine(moveHandsCoroutine);
            }

            moveHandsCoroutine = Controller.StartCoroutine(UpdateRig(boxRig, value, Data.HoldBoxTransitionDuration));
        }

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
        internal void Run() => Play(runState);
        internal void Crounch() => Play(crouchState);
        internal void CrounchWalk() => Play(crouchWalkState);

        private void Play(string stateName, float transition = 0.25f, int layerIndex = 0)
        {
            animator.CrossFadeInFixedTime(stateName, transition, layerIndex);
        }

        private void SetFloat(string parameter, float value) => animator.SetFloat(parameter, value);

        private IEnumerator UpdateRig(Rig rig, float targetWeight, float duration)
        {
            while (rig.weight != targetWeight)
            {
                rig.weight = Mathf.MoveTowards(rig.weight, targetWeight, 1f / duration * Time.deltaTime);
                yield return null;
            }
        }

        public void ShowPlayer() => SetMaterial(defaultMaterial);
        public void HidePlayer() => SetMaterial(invisibleMaterial);
        private void SetMaterial(Material newMaterial)
        {
            Material[] materials = new Material[1] { newMaterial };
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshes)
            {
                skinnedMeshRenderer.material = newMaterial;
            }
        }
    }
}