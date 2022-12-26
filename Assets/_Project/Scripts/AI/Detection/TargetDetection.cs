using UnityEngine;

namespace TriplanoTest.AI
{
    public abstract class TargetDetection : MonoBehaviour
    {
        public bool FindTargetInsideRange(out Transform target)
        {
            target = FindTargetInsideRange()?.transform;
            return target;
        }

        public abstract Rigidbody FindTargetInsideRange();
    }
}