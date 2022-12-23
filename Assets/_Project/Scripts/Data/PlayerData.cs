using TriplanoTest.Shared;
using UnityEngine;

namespace TriplanoTest.Data
{
    [CreateAssetMenu(menuName = MenuPath.Data + "Player", fileName = "New" + nameof(PlayerData))]
    public class PlayerData : ScriptableObject
    {
        [Header("Physics")]
        [SerializeField] private float groundCheckRadius;
        [SerializeField] private float fullLockAngle;
        [SerializeField] private float aimBodyCorrectionSpeed;
        [SerializeField] private float mass;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;
        [SerializeField] private Vector2 cameraRangeRotation;
        [SerializeField] private float rotationSmoothTime;
        [SerializeField] private int maxFallingVelocity;

        public float GroundCheckRadius => groundCheckRadius;
        public float FullLockAngle => fullLockAngle;
        public float AimBodyCorrectionSpeed => aimBodyCorrectionSpeed;
        public float Mass => mass;
        public float Acceleration => acceleration;
        public float Deceleration => deceleration;
        public Vector2 CameraRangeRotation => cameraRangeRotation;
        public float RotationSmoothTime => rotationSmoothTime;
        public int MaxFallingVelocity => maxFallingVelocity;

        public float Sensitivity { get; set; }
    }
}