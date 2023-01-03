using TriplanoTest.Shared;
using TriplanoTest.Shared.Design;
using UnityEngine;

namespace TriplanoTest.Data
{
    [CreateAssetMenu(menuName = MenuPath.Data + "Guard", fileName = "New" + nameof(GuardData))]
    public class GuardData : ScriptableObject, IHasIcon
    {
        [Header("Guard")]
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float waitTime = 3f;
        [SerializeField] private float rotationSpeed = 3f;
        [SerializeField] private float animationSpeed = 1f;

        public IconType IconType => IconType.Body;

        public float MoveSpeed => moveSpeed;
        public float WaitTime => waitTime;
        public float RotationSpeed => rotationSpeed;
        public float AnimationSpeed => animationSpeed;
    }
}