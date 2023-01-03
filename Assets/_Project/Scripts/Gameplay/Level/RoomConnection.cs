using UnityEngine;
using System.Text.RegularExpressions;
using TriplanoTest.Shared;
using UnityEngine.AddressableAssets;

namespace TriplanoTest.Gameplay.Level
{
    public class RoomConnection : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [Space]
        [SerializeField] private string connectionName;
        [SerializeField] private string nextConnectionName;
        [AssetReferenceUILabelRestriction(Constants.Scene)]
        [SerializeField] private AssetReference nextLevel;

        public Transform SpawnPoint => spawnPoint;
        public string ConnectionName => connectionName;

        private void OnValidate()
        {
            string name = "Connection";

            if (!string.IsNullOrEmpty(connectionName))
            {
                name += $"[{connectionName}]";

                string nextLevelName = nextLevel.ToString();

                if (!string.IsNullOrEmpty(nextLevelName))
                {
                    nextLevelName = Regex.Replace(nextLevelName, @"\[\w+\]", string.Empty);

                    nextLevelName = nextLevelName.Replace(" (UnityEngine.SceneAsset)", string.Empty);
                    name += $" -> [{nextLevelName}/{nextConnectionName}]";
                }
            }

            this.name = name;
        }

        private void OnTriggerEnter(Collider other)
        {
            LevelManager.LoadNewArea(nextLevel, nextConnectionName);
        }
    }
}