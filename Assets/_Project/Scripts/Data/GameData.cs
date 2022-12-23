using TriplanoTest.Shared;
using UnityEngine;

namespace TriplanoTest.Data
{
    /// <summary>
    /// Note to developers: Please describe what this class does.
    /// </summary>
    [CreateAssetMenu(menuName = MenuPath.Data + "Game", fileName = "New" + nameof(GameData))]
    public class GameData : ScriptableObject 
    {
        [Header("Game Data")]
        [SerializeField] private PlayerData player;

        public static PlayerData Player => Instance.player;

        public static GameData Instance { get; private set; }

        public void Initialize()
        {
            Instance = this;
        }
    }
}