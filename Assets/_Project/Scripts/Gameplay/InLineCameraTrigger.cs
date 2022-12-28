using Mono.Cecil.Cil;
using TriplanoTest.Shared.ExtensionMethods;
using UnityEngine;

namespace TriplanoTest.Gameplay
{
    public class InLineCameraTrigger : CameraTriggerActivator
    {
        [Header("Camera Parameters")]
        [Range(0, .5f)]
        [SerializeField] private float strenght;

        [Header("Components")]
        [SerializeField] private BoxCollider stageCollider;
        [SerializeField] private Transform cameraContainer;

        private Transform PlayerTransform => MainCamera.PlayerTarget;

        private Vector3 lineStart;
        private Vector3 lineEnd;

        private bool Active => virtualCamera.gameObject.activeSelf;

        private void Awake()
        {
            CalculateLinePoints();
        }

        /// <summary>
        /// Calculate the starting position of the line, which is the most front and back point of the BoxCollider (relative to its rotation)
        /// </summary>
        private void CalculateLinePoints()
        {
            Vector3 center = stageCollider.center;
            float halfSize = stageCollider.size.z / 2;
            lineStart = stageCollider.transform.TransformPoint(center + Vector3.forward * halfSize);
            lineEnd = stageCollider.transform.TransformPoint(center + Vector3.forward * -halfSize);
        }

        private void Update()
        {
            if (!Active)
                return;

            Vector3 desiredPoint = GetClosestPointInLine(PlayerTransform.position);

            float distanceTotal = Vector3.Distance(lineStart, lineEnd);

            float distanceCurrent = Vector3.Distance(desiredPoint, lineStart);

            float transition = distanceCurrent / distanceTotal;

            transition = transition.Remap(0f, 1f, 0 + strenght, 1 - strenght);

            Vector3 offset = transform.rotation * cameraContainer.localPosition;
            virtualCamera.transform.position = Vector3.Lerp(lineStart, lineEnd, transition) + offset;
        }

        public Vector3 GetClosestPointInLine(Vector3 worldPoint)
        {
            Vector3 direction = lineEnd - lineStart;
            Vector3 pointToWorldPoint = worldPoint - lineStart;
            float scalar = Vector3.Dot(direction, pointToWorldPoint) / direction.sqrMagnitude;

            if (scalar < 0)
            {
                return lineStart;
            }
            else if (scalar > 1)
            {
                return lineEnd;
            }
            else
            {
                return lineStart + scalar * direction;
            }
        }

        private void OnDrawGizmos()
        {
            CalculateLinePoints();

            Gizmos.color = Color.red;
            Gizmos.DrawLine(lineStart, lineEnd);

            if (!Application.isPlaying || !Active)
                return;

            Vector3 point = GetClosestPointInLine(PlayerTransform.position);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(point, PlayerTransform.position);
        }
    }
}