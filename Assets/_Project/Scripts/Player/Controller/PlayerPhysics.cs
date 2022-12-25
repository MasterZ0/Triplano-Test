using UnityEngine;
using System;
using TriplanoTest.Data;
using TriplanoTest.Shared.Utils;
using TriplanoTest.Gameplay;
using System.Linq;

namespace TriplanoTest.Player
{
    /// <summary>
    /// Handles player physics
    /// </summary>
    [Serializable]
    public sealed class PlayerPhysics : PlayerControllerComponent
    {
        [Header("Layers")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask interactableLayer;

        [Header("Components")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private Transform interactableCheckPoint;

        private Vector3 velocity;

        private float gravityScale;
        private float moveSpeed;
        private float verticalVelocity;
        private float targetYRotation;
        private float rotationVelocity;

        public IPushable CurrentPushable { get; private set; }
        public Vector3 Position => characterController.transform.position;

        public Vector3 Velocity => characterController.velocity;
        public Transform Transform => characterController.transform;
        private PlayerData Data => GameData.Player;
        private float Weight => Data.Mass * Physics.gravity.y;
        private float EulerYCamera => Controller.Camera.CameraTarget.eulerAngles.y;

        internal bool TryInteract()
        {
            CurrentPushable = null;
            Collider[] colliders = Physics.OverlapSphere(interactableCheckPoint.position, Data.InteractCheckRadius, interactableLayer);

            foreach (Collider col in colliders)
            {
                if (col.attachedRigidbody && col.attachedRigidbody.TryGetComponent(out IPushable interactable))
                {
                    CurrentPushable = interactable;
                    return true;
                }
            }

            return false;
        }

        public bool CheckGround()
        {
            return Physics.CheckSphere(groundCheckPoint.position, Data.GroundCheckRadius, groundLayer);
        }

        public void Jump(float jumpHeight)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Weight);
        }

        public void SetGravityScale(float gravityScale) => this.gravityScale = gravityScale;

        internal void FixedMove(float speed)
        {
            // Movement
            Vector2 direction = Controller.Inputs.Move;

            if (direction != Vector2.zero)
            {
                moveSpeed = speed;
                if (direction.magnitude > 1)
                {
                    direction = direction.normalized;
                }
                targetYRotation = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + EulerYCamera;
            }

            // Rotation
            if (!Controller.Camera.YLocked)
            {
                float rotation = Mathf.MoveTowardsAngle(Transform.eulerAngles.y, EulerYCamera, Data.AimBodyCorrectionSpeed);
                Transform.rotation = Quaternion.Euler(0f, rotation, 0f);

                if (MathUtils.AngleDiference(EulerYCamera, Transform.eulerAngles.y) <= Data.FullLockAngle)
                {
                    Controller.Camera.LockY(true);
                }
            }
            else
            {
                float yRotation = Controller.Inputs.Look.x * Data.MouseSensitivity + Transform.eulerAngles.y;
                Transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            }
        }

        private bool acceleration;

        internal void MoveTo(Vector3 targetPoint, Vector3 lookAt, float speed)
        {
            // Direction the body will move
            Vector3 direction = (targetPoint - Transform.position).normalized;
            targetYRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Look at target
            float targetYAngle = Mathf.Atan2(lookAt.x, lookAt.z) * Mathf.Rad2Deg;
            float yRotation = Mathf.SmoothDampAngle(Transform.eulerAngles.y, targetYAngle, ref rotationVelocity, Data.RotationSmoothTime);
            Transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            // Limits the player's speed so that he doesn't go over the desired point. Ignore Y distance
            var distance = targetPoint - Transform.position;
            distance.y = 0f;
            float speed2 = distance.magnitude / Time.fixedDeltaTime;
            moveSpeed = Mathf.Min(speed, speed2);

            acceleration = false;
        }

        internal void MovePush()
        {
            Vector2 direction = Controller.Inputs.Move;

            if (direction.magnitude > 1)
            {
                direction = direction.normalized;
            }

            moveSpeed = Data.MoveSpeedPushing;
            targetYRotation = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + EulerYCamera;

            Vector3 targetDirection = Quaternion.Euler(0f, targetYRotation, 0f) * Vector3.forward;
            CurrentPushable.Push(targetDirection * moveSpeed * Time.fixedDeltaTime);
            acceleration = false;
        }

        internal void Move(float speed)
        {
            Vector2 direction = Controller.Inputs.Move;

            if (direction == Vector2.zero)
                return;

            moveSpeed = speed;
            if (direction.magnitude > 1)
            {
                direction = direction.normalized;
            }

            targetYRotation = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + EulerYCamera;

            // Rotation
            float rotation = Mathf.SmoothDampAngle(Transform.eulerAngles.y, targetYRotation, ref rotationVelocity, Data.RotationSmoothTime);
            Transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        }

        internal void Update()
        {
            UpdateAccelerationSpeed();
            UpdateVerticalVelocity();
            characterController.Move(velocity * Time.fixedDeltaTime);
        }

        // ---------- Private Methods

        /// <summary> Gravity and Jump Velocity </summary>
        private void UpdateVerticalVelocity()
        {
            if (CheckGround() && verticalVelocity < 0f) // Slope force?
            {
                verticalVelocity = -2f;
            }

            float maxFallingVelocity = -Data.MaxFallingVelocity;

            verticalVelocity += Weight * gravityScale * Time.fixedDeltaTime;
            if (verticalVelocity < maxFallingVelocity)
            {
                verticalVelocity = maxFallingVelocity;
            }

            velocity.y = verticalVelocity;
        }

        private void UpdateAccelerationSpeed()
        {
            // Reset state speed
            float speed;

            if (acceleration)
            {
                // Get the player's speed in a plane
                float currentHorizontalSpeed = new Vector3(Velocity.x, 0f, Velocity.z).magnitude;

                // Get Acceleration or Deceleration transition
                float transition = moveSpeed > currentHorizontalSpeed ? Data.Acceleration : Data.Deceleration;

                // Interpolate between current horizontal speed and target speed
                speed = Mathf.Lerp(currentHorizontalSpeed, moveSpeed, transition * Time.fixedDeltaTime);
                speed = Mathf.Min(moveSpeed, speed);
            }
            else
            {
                speed = moveSpeed;
                acceleration = true;
            }

            moveSpeed = 0f;

            // Update forward velocity
            Vector3 targetDirection = Quaternion.Euler(0f, targetYRotation, 0f) * Vector3.forward;
            velocity = targetDirection.normalized * speed;
        }

        public void DrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(groundCheckPoint.position, Data.GroundCheckRadius);
            Gizmos.DrawWireSphere(interactableCheckPoint.position, Data.InteractCheckRadius);
        }
    }
}