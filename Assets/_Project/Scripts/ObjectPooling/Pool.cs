using System.Collections.Generic;
using UnityEngine;

namespace TriplanoTest.ObjectPooling
{
    public class Pool
    {
        public int AvailableInstances => pool.Count;
        public Transform Container { get; }
        public List<Component> ActiveInstances { get; } = new List<Component>();

        private readonly Queue<Component> pool = new Queue<Component>();

        public Pool(string name, Transform poolContainer)
        {
            Container = new GameObject($"Container [{name}]").transform;
            Container.SetParent(poolContainer);
        }

        public Component GetFromPool()
        {
            Component component = null;
            
            while (component == null && AvailableInstances > 0)
                component = pool.Dequeue();

            if (component != null)
            {
                ActiveInstances.Add(component);
            }

            return component;
        }

        public void AddToPool(Component component)
        {
            if (!component)
            {
                Debug.LogError(ObjectPool.NullError);
                return;
            }                
            
            if (ActiveInstances.Contains(component))
                ActiveInstances.Remove(component);

            component.gameObject.SetActive(false);
            component.transform.SetParent(Container);
            pool.Enqueue(component);
        }

        public void AddActiveInstance(Component component)
        {
            ActiveInstances.Add(component);
        }
    }
}
