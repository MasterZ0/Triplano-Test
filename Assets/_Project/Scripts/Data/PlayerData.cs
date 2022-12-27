using TriplanoTest.Shared;
using UnityEngine;

namespace TriplanoTest.Data
{

    [CreateAssetMenu(menuName = MenuPath.Data + "Player", fileName = "New" + nameof(PlayerData))]
    public class PlayerData : ScriptableObject
    {
        [Header("Physics")]
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private float interactCheckRadius = 0.3f;
        [SerializeField] private float fullLockAngle = 15f;
        [SerializeField] private float aimBodyCorrectionSpeed = 15f;

        [SerializeField] private float standHeight = 1.5f;
        [SerializeField] private float crouchHeight = 0.8f;

        [Header(" - Movement")]
        [SerializeField] private float walkSpeed = 4f;
        [SerializeField] private float crouchSpeed = 4f;
        [SerializeField] private float moveSpeedPushing = 2f;
        [SerializeField] private float acceleration = 20f;
        [SerializeField] private float deceleration = 15f;
        [SerializeField] private float pushBoxOffset = 0.5f;

        [Header(" - Rotation")]
        [SerializeField] private float mouseSensitivity = 0.2f;
        [SerializeField] private float rotationSmoothTime = 0.12f;
        [SerializeField] private Vector2 cameraRangeRotation = new Vector2(-35f, 75f);

        [Header(" - Jump")]
        [SerializeField] private float jumpVelocity = 1f;
        [SerializeField] private Vector2 jumpRangeDuration = new Vector2(-0.04f, 0.25f);

        [Header(" - Gravity")]
        [SerializeField] private float groundGravity = 3f;
        [SerializeField] private float fallingGravity = 3f;
        [SerializeField] private float jumpGravity = 1f;
        [SerializeField] private float mass = 1f;
        [SerializeField] private float maxFallingVelocity = 11f;

        [Header(" - Visual")]
        [SerializeField] private float holdBoxTransitionDuration = 1f;
        [SerializeField] private float animationBlendDamp = 0.1f;

        public float InteractCheckRadius => interactCheckRadius;
        public float GroundCheckRadius => groundCheckRadius;
        public float FullLockAngle => fullLockAngle;
        public float AimBodyCorrectionSpeed => aimBodyCorrectionSpeed;
        public float CrouchHeight => crouchHeight;
        public float StandHeight => standHeight;
        public float Mass => mass;
        public float Acceleration => acceleration;
        public float Deceleration => deceleration;
        public Vector2 CameraRangeRotation => cameraRangeRotation;
        public float RotationSmoothTime => rotationSmoothTime;
        public float MaxFallingVelocity => maxFallingVelocity;

        public float MouseSensitivity => mouseSensitivity;
        public float FallingGravity => fallingGravity;
        public float JumpGravity => jumpGravity;
        public float JumpVelocity => jumpVelocity;
        public Vector2 JumpRangeDuration => jumpRangeDuration;

        public float WalkSpeed => walkSpeed;
        public float CrouchSpeed => crouchSpeed;

        public float GroundGravity => groundGravity;

        public float PushBoxOffset => pushBoxOffset;
        public float MoveSpeedPushing => moveSpeedPushing;

        public float HoldBoxTransitionDuration => holdBoxTransitionDuration;
        public float AnimationBlendDamp => animationBlendDamp;
    }
}