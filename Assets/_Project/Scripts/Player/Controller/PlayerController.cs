using UnityEngine;
using TriplanoTest.Player.FSM;
using TriplanoTest.Inputs;
using TriplanoTest.Data;
using TriplanoTest.Gameplay;
using TriplanoTest.Shared;

namespace TriplanoTest.Player
{
    public sealed class PlayerController : MonoBehaviour, ICollector, IPlayer
    {
        [Header("Player Controller")]
        [Space]
        [SerializeField] private GameEvent<bool> onPauseGame;
        [Space]
        [SerializeField] private PlayerDebug playerDebug;
        [SerializeField] private PlayerPhysics playerPhysics;
        [SerializeField] private PlayerAnimator playerAnimator;
        [SerializeField] private PlayerCamera playerCamera;
        [SerializeField] private PlayerSFX playerSFX;
        [SerializeField] private PlayerUI playerUI;

        public PlayerDebug Debug => playerDebug;
        public PlayerData Data => GameData.Player;
        public PlayerPhysics Physics => playerPhysics;
        public PlayerAnimator Animator => playerAnimator;
        public PlayerSFX SFX => playerSFX;
        public PlayerCamera Camera => playerCamera;
        public PlayerInputs Inputs => playerInputs;

        private PlayerInputs playerInputs;
        private PlayerFSM stateMachine;

        private void Awake()
        {
            playerUI.Init(this);
            playerPhysics.Init(this);
            playerAnimator.Init(this);
            playerCamera.Init(this);
            playerSFX.Init(this);
            playerDebug.Init(this);

            playerInputs = new PlayerInputs();
            stateMachine = PlayerFSM.Create<IdlePS>(this);

            onPauseGame += OnPauseGame;
        }

        private void OnDestroy()
        {
            playerDebug.Destroy();

            playerInputs.Dispose();
            onPauseGame -= OnPauseGame;
        }

        public void AddCoin(int amount) => playerUI.AddCoin(amount);
        public void SetPosition(Transform point) => Physics.SetPosition(point);
        public void SetActiveInput(bool active) => playerInputs.SetActive(active);

        private void OnPauseGame(bool paused) => SetActiveInput(!paused);

        private void FixedUpdate()
        {
            stateMachine.Update();
            playerPhysics.Update();
            playerCamera.Update();
            playerAnimator.Update();
            playerUI.Update();
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                stateMachine.DrawGizmos();
            }
        }

        private void OnDrawGizmosSelected()
        {
            playerPhysics.DrawGizmos();
        }
    }
}