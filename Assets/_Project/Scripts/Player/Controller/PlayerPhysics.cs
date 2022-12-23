using UnityEngine;
using System;
using TriplanoTest.Data;
using TriplanoTest.Shared.Utils;

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

        [Header("Components")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform groundCheck;

        private float gravityScale;
        private float moveSpeed;
        private float verticalVelocity;
        private float targetYRotation;
        private float rotationVelocity;

        #region Properties and Const
        public Vector3 Velocity => characterController.velocity;

        private Transform Transform => characterController.transform;
        private PlayerData Data => Controller.Data;
        private float Weight => Data.Mass * Physics.gravity.y;
        private float EulerYCamera => Controller.Camera.CameraTarget.eulerAngles.y;
        #endregion

        public void SetGravityScale(float gravityScale) => this.gravityScale = gravityScale;


        #region Public methods
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
                float yRotation = Controller.Inputs.Look.x * Data.Sensitivity + Transform.eulerAngles.y;
                Transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            }
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
            UpdateVerticalVelocity();
            float speed = GetAccelerationSpeed(moveSpeed);

            // Movement X Z
            Vector3 targetDirection = Quaternion.Euler(0f, targetYRotation, 0f) * Vector3.forward;
            Vector3 velocity = targetDirection.normalized * speed;
            velocity.y = verticalVelocity;

            characterController.Move(velocity * Time.fixedDeltaTime);

            moveSpeed = 0f;
        }

        public void Jump(float jumpHeight) // TODO: What is -2?
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Weight);
        }

        public bool CheckGround()
        {
            return Physics.CheckSphere(groundCheck.position, Data.GroundCheckRadius, groundLayer);
        }
        #endregion

        #region Private Methods
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
        }

        private float GetAccelerationSpeed(float targetSpeed) // TODO: acceleration = 6, deceleration = 3
        {
            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(Velocity.x, 0f, Velocity.z).magnitude;

            //float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
            float inputMagnitude = 1f;
            float transition = targetSpeed > currentHorizontalSpeed ? Data.Acceleration : Data.Deceleration;

            return Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, transition * Time.fixedDeltaTime);
        }
        #endregion

        #region Gizmos
        public void DrawGizmos()
        {
            Gizmos.DrawWireSphere(groundCheck.position, Data.GroundCheckRadius);
        }
        #endregion

    }
}