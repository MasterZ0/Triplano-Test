using TriplanoTest.Shared;
using TriplanoTest.Shared.Design;
using UnityEngine;

namespace TriplanoTest.Data
{
    /// <summary>
    /// Store all game datas
    /// </summary>
    [CreateAssetMenu(menuName = MenuPath.Data + "Game", fileName = "New" + nameof(GameData))]
    public class GameData : ScriptableObject, IHasIcon
    {
        [Header("Game Data")]
        [SerializeField] private PlayerData player;
        [SerializeField] private GeneralData general;

        public static PlayerData Player => Instance.player;
        public static GeneralData General => Instance.general;

        public static GameData Instance { get; private set; }

        IconType IHasIcon.IconType => IconType.AudioMixerController;

        private void OnValidate() => Initialize();

        public void Initialize()
        {
            Instance = this;
        }
    }
}