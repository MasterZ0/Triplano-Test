using TriplanoTest.Shared;
using UnityEngine;

namespace TriplanoTest.Data
{
    [CreateAssetMenu(menuName = MenuPath.Data + "General", fileName = "New" + nameof(GeneralData))]
    public class GeneralData : ScriptableObject
    {
        [Header("General")]
        [SerializeField] private float boxMoveDistance = 1f;

        public float BoxMoveDistance => boxMoveDistance;
    }
}