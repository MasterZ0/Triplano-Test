using UnityEngine;

namespace TriplanoTest.Persistence
{
    /// <summary>
    /// Save the GameObject's position and rotation, and load it automatically at Awake
    /// </summary>
    public class OrientationState : PersistentState<OrientationData> 
    {
        public override OrientationData DefaultState => transform;
        
        public override void LoadState()
        {
            base.LoadState();
            transform.position = CurrentState;
            transform.rotation = CurrentState;
        }

        public override void SaveState()
        {
            CurrentState = transform;
            base.SaveState();
        }
    }

    [System.Serializable]
    public class OrientationData 
    {
        public float positionX;
        public float positionY;
        public float positionZ;

        public float rotationX;
        public float rotationY;
        public float rotationZ;
        public float rotationW;

        public OrientationData() { }
        
        public OrientationData(Transform transform)
        {
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;

            positionX = position.x;
            positionY = position.y;
            positionZ = position.z;

            rotationX = rotation.x;
            rotationY = rotation.y;
            rotationZ = rotation.z;
            rotationW = rotation.w;
        }

        public static implicit operator Quaternion(OrientationData data)
        {
            return new Quaternion(data.rotationX, data.rotationY, data.rotationZ, data.rotationW);
        }

        public static implicit operator Vector3(OrientationData data)
        {
            return new Vector3(data.positionX, data.positionY, data.positionZ);
        }

        public static implicit operator OrientationData(Transform transform)
        {
            return new OrientationData(transform);
        }
    }
}