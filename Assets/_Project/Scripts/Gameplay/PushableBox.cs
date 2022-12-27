using TriplanoTest.Data;
using UnityEngine;

namespace TriplanoTest.Gameplay
{
    /// <summary>
    /// Note to developers: Please describe what this MonoBehaviour does.
    /// </summary>
    public class PushableBox : MonoBehaviour, IPushable
    {
        [Header("PushableBox")] 
        [SerializeField] private float moveDistance = 0.5f;
        [SerializeField] private LayerMask scenary;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private BoxCollider boxCollider;

        public bool IsMoving { get; private set; }
        public Vector3 Position => transform.position;

        private Vector3 targetPosition;
        private float speed;

        /// <returns> Nearest edge </returns>
        public Vector3 GetHoldPoint(Transform holderPivot, out Vector3 direction)
        {
            // Calculate the distance between the player and the box
            Vector3 distance = holderPivot.position - transform.position;

            // Adjust X or Z position based on the player's position relative to the box
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.z))
            {
                direction = distance.x > 0 ? Vector3.right : Vector3.left;
                return new Vector3
                {
                    x = transform.position.x + direction.x * boxCollider.size.x / 2,
                    y = transform.position.y,
                    z = holderPivot.position.z,
                };
            }
            else
            {
                direction = distance.z > 0 ? Vector3.forward : Vector3.back;
                return new Vector3
                {
                    x = holderPivot.position.x,
                    y = transform.position.y,
                    z = transform.position.z + direction.z * boxCollider.size.z / 2
                };
            }
        }

        public bool Push(Vector3 direction, float speed)
        {
            direction = direction.normalized;

            // Check if the box is falling
            if (rigidbody.velocity != Vector3.zero || CanMoveInAxis(Vector3.down)) 
                return false;

            if (!CanMoveInAxis(direction))
                return false;

            //rigidbody.isKinematic = true;
            IsMoving = true;
            this.speed = speed;
            // Move the box by the distanceMoved vector
            targetPosition = transform.position + direction * moveDistance;

            return true;
        }

        private bool CanMoveInAxis(Vector3 direction)
        {
            // Do a BoxCast from the closest face
            Vector3 start = transform.position + boxCollider.center;
            RaycastHit[] cols = Physics.BoxCastAll(start, boxCollider.size / 2, direction, transform.rotation, moveDistance, scenary);

            foreach (RaycastHit col in cols)
            {
                // If has obstacle don't move the box
                if (col.collider != boxCollider)
                {
                    Debug.DrawLine(start, cols[0].point, Color.red, 2f);
                    return false;
                }
            }

            return true;
        }

        private void FixedUpdate()
        {
            if (!IsMoving)
                return;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);

            if (Vector3.Distance(transform.position, targetPosition) <= 0.02f)
            {
                transform.position = targetPosition;
                IsMoving = false;
                //rigidbody.isKinematic = false;
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 size = boxCollider.size + Vector3.one * GameData.Player.PushBoxOffset * 2f;
            size.y = .2f;
            Gizmos.DrawWireCube(transform.position, size);
        }
    }
}