using TriplanoTest.Data;
using UnityEngine;

namespace TriplanoTest.Gameplay
{
    /// <summary>
    /// Note to developers: Please describe what this MonoBehaviour does.
    /// </summary>
    public class PushableBox : MonoBehaviour, IPushable
    {
        [Header("PushableBox")] // Remember to use attributes, #regions and XML Documentation :)
        [SerializeField] private BoxCollider boxCollider;

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

        public bool Push(Vector3 distance)
        {
            transform.position += distance;
            return true;
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