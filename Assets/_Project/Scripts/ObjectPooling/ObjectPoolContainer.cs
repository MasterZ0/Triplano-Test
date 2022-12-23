using UnityEngine;
using Object = UnityEngine.Object;

namespace TriplanoTest.ObjectPooling
{
    public class ObjectPoolContainer : MonoBehaviour
    {
        private static ObjectPoolContainer instance;

        public static Transform SpawnContainer => Instance.transform;
        public static ObjectPoolContainer Instance
        {
            get
            {
                if (instance == null) 
                    CreateInstance();
                
                return instance;
            }
            
            private set => instance = value;
        }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            foreach (Transform item in transform)
            {
                item.ReturnToPool();
            }
        }

        public static GameObject GenerateContainer(Object prefab)
        {
            return new GameObject($"[{prefab.name} Container]");
        }

        public static void CreateInstance()
        {
            GameObject pool = new GameObject("ObjectPoolContainer [Generated]");
            ObjectPoolContainer objectPool = pool.AddComponent<ObjectPoolContainer>();
            instance = objectPool;
        }
    }
}