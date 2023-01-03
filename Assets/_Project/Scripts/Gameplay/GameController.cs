using TriplanoTest.Shared;
using UnityEngine;
using TriplanoTest.Gameplay.Level;
using UnityEngine.AddressableAssets;
using TriplanoTest.Inputs;

namespace TriplanoTest.Gameplay
{

    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameEvent onGuardFoundPlayer;
        [SerializeField] private GameEvent onVictory;
        [SerializeField] private GameObject dialogue;
        [SerializeField] private Transform playerController;

        [AssetReferenceUILabelRestriction(Constants.Scene)]
        [SerializeField] private AssetReference startLevel;
        [SerializeField] private string startConnection;

        private UIInputs uiInputs;
        private bool playerLose;
        private IPlayer player;

        private void Awake()
        {
            player = playerController.GetComponent<IPlayer>();

            uiInputs = new UIInputs(false);
            uiInputs.OnSubmit += OnSubmit;

            LevelManager.onLoadStart += OnLoadStart;
            LevelManager.onUnloadFinish += OnUnloadFinish;
            LevelManager.onLoadRoom += OnLoadRoom;
            LevelManager.onLoadFinish += OnLoadFinish;

            onGuardFoundPlayer += OnGuardFoundPlayer;
            onVictory += OnVictory;
        }

        private void OnDestroy()
        {
            uiInputs.Dispose();

            LevelManager.onLoadStart -= OnLoadStart;
            LevelManager.onUnloadFinish -= OnUnloadFinish;
            LevelManager.onLoadRoom -= OnLoadRoom;
            LevelManager.onLoadFinish -= OnLoadFinish;

            onGuardFoundPlayer -= OnGuardFoundPlayer;
            onVictory -= OnVictory;
        }

        /// <summary> Called after dialogue open </summary>
        public void OnActiveInput() => uiInputs.SetActive(true);

        /// <summary> Called after confirm dialogue </summary>
        private void OnSubmit()
        {
            uiInputs.SetActive(false);
            LevelManager.LoadNewArea(startLevel, startConnection);
        }

        /// <summary> Disable player </summary>
        private void OnLoadStart() => player.SetActiveInput(false);


        /// <summary> Hide dialogue box </summary>
        private void OnUnloadFinish()
        {
            player.SetActiveInput(true);
            dialogue.SetActive(false);
        }

        /// <summary> Update player position </summary>
        private void OnLoadRoom(Room room, RoomConnection connectionPoint)
        {
            player.SetPosition(connectionPoint.SpawnPoint);
        }

        /// <summary> Player can be founded again and active inputs </summary>
        private void OnLoadFinish()
        {
            playerLose = false;
            player.SetActiveInput(true);
        }

        /// <summary> Display dialogue box </summary>
        private void OnGuardFoundPlayer()
        {
            if (playerLose)
                return;

            player.SetActiveInput(false);
            dialogue.SetActive(true);
            playerLose = true;
        }

        /// <summary> Disable inputs and change scene </summary>
        public void OnVictory()
        {
            player.SetActiveInput(false);
            LevelManager.PlayerVictory();
        }
    }
}