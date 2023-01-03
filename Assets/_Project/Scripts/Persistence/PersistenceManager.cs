using System.Collections.Generic;
using UnityEngine;

namespace TriplanoTest.Persistence
{
    /// <summary>
    /// Handles data storage
    /// </summary>
    public static class PersistenceManager
    {
        private static readonly Dictionary<string, object> temporaryData = new();

        /// <summary>
        /// Gey key to access volatile memory
        /// </summary>
        public static string GetTypeKey(GameObject gameObject) => $"{gameObject.transform.root.name}/{gameObject.name}";

        /// <summary>
        /// Save data to volatile memory
        /// </summary>
        public static void Save<T>(string key, T data)
        {
            temporaryData[key] = data;
        }

        /// <summary>
        /// Load data from volatile memory
        /// </summary>
        public static T Load<T>(string key, T defaultValue = default)
        {
            T loadedValue = defaultValue;

            if (temporaryData.ContainsKey(key))
            {
                return (T)temporaryData[key];
            }

            return loadedValue;
        }

        /// <summary>
        /// Clear volatile memory
        /// </summary>
        public static void ClearData()
        {
            temporaryData.Clear();
        }
    }
}