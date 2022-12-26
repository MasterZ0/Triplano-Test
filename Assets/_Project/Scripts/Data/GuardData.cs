using TriplanoTest.Shared;
using UnityEngine;

namespace TriplanoTest.Data
{
    [CreateAssetMenu(menuName = MenuPath.Data + "Guard", fileName = "New" + nameof(GuardData))]
    public class GuardData : ScriptableObject
    {
        [Header("Guard")]
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float waitTime = 3f;

        public float MoveSpeed => moveSpeed;
        public float WaitTime => waitTime;
    }
}