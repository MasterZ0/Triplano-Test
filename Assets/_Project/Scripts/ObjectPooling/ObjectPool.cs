using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TriplanoTest.ObjectPooling 
{
    /// <summary>
    /// Manages game resources for processing gain (better garbage collection usage)
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        private static readonly Dictionary<GameObject, Pool> pools = new Dictionary<GameObject, Pool>();
        private static readonly Dictionary<GameObject, Component> components = new Dictionary<GameObject, Component>();

        private static Transform poolContainer;
        private static Transform spawnContainer => ObjectPoolContainer.SpawnContainer;

        public const string NullError = "Danger! Null object sent to pool";

        private void Awake()
        {
            if (poolContainer != null)
                Destroy(gameObject);

            poolContainer = transform;
        }   

        public static GameObject SpawnPooledObject(GameObject prefab, Transform parent) 
        {
            return SpawnPooledObject(prefab, default, default, parent);
        }

        public static GameObject SpawnPooledObject(GameObject prefab, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            return SpawnPooledObject(prefab.transform, position, rotation, parent).gameObject;
        }

        public static T SpawnPooledObject<T>(T prefab, Transform parent) where T : Component
        {
            return SpawnPooledObject(prefab, default, default, parent);
        }

        public static T SpawnPooledObject<T>(T prefab, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : Component
        {
            if (!parent)
            {
                parent = spawnContainer;
            }

            if (!pools.ContainsKey(prefab.gameObject))
            {
                Pool createdPool = new Pool(prefab.name, poolContainer);
                T instance = Instantiate(prefab, position, rotation, parent);

                pools[prefab.gameObject] = createdPool;
                pools[instance.gameObject] = createdPool;
                createdPool.AddActiveInstance(instance);

                components[instance.gameObject] = instance;
                return instance;
            }

            Pool pool = pools[prefab.gameObject];

            if (pool.AvailableInstances > 0)
            {
                Component poolComponent = pool.GetFromPool();
                T component = poolComponent as T;

                if (component == null)
                    component = poolComponent.GetComponent<T>();

                component.transform.SetPositionAndRotation(position, rotation);
                component.transform.SetParent(parent);
                component.gameObject.SetActive(true);

                return component;
            }
            else
            {
                T component = Instantiate(prefab, position, rotation, parent);
                GameObject componentGameObject = component.gameObject;
                components[componentGameObject] = component;
                pools[componentGameObject] = pool;
                pool.AddActiveInstance(component);
                return component;
            }
        }

        public static void ReturnToPool(GameObject instance) => ReturnToPool(instance.transform);

        public static void ReturnToPool<T>(T instance) where T : Component
        {
            if (pools.ContainsKey(instance.gameObject))
            {
                pools[instance.gameObject].AddToPool(components[instance.gameObject]);
                return;
            }

            GameObject instanceGameObject = instance.gameObject;
            components[instanceGameObject] = instance;
            Pool pool = new Pool(instance.name, poolContainer);
            pools[instanceGameObject] = pool;
            pool.AddToPool(instance);
        }

        public static void ReturnAllToPool()
        {
            foreach (Pool pool in pools.Values.ToList())
            {
                foreach (Component instance in pool.ActiveInstances.ToList())
                {
                    if (!instance)
                    {
                        pool.ActiveInstances.Remove(instance);
                        continue;
                    }
                        
                    pool.AddToPool(instance);
                }
            }
        }

        private void OnDestroy()
        {
            pools.Clear();
        }
    }
}