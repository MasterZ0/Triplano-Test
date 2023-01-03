using UnityEngine;

namespace TriplanoTest.Persistence
{
    /// <summary>
    /// Automation to save data using a MonoBehaviour
    /// </summary>
    /// <typeparam name="T">Serialized data that will be saved</typeparam>
    public abstract class PersistentState<T> : MonoBehaviour
    {
        [Tooltip("Save when destroy the GameObject")]
        [SerializeField] private bool saveOnDestroy = true;

        public T CurrentState { get; set; }
        public abstract T DefaultState { get; }
        
        private string keyName;

        private void Start()
        {
            keyName = PersistenceManager.GetTypeKey(gameObject);
            LoadState();
        }

        private void OnDestroy()
        {
            if (saveOnDestroy)
                SaveState();
        }

        public virtual void LoadState()
        {
            CurrentState = PersistenceManager.Load(keyName, DefaultState);
        }

        public virtual void SaveState()
        {
            PersistenceManager.Save(keyName, CurrentState);
        }
    }
}