using UnityEngine;
using TriplanoTest.Player.FSM;
using TriplanoTest.Inputs;
using TriplanoTest.Data;
using TriplanoTest.Gameplay;

namespace TriplanoTest.Player
{
    public sealed class PlayerController : MonoBehaviour, ICollector
    {
        [Header("Player Controller")]
        [SerializeField] private bool debugMode;
        [Space]
        [SerializeField] private PlayerPhysics playerPhysics;
        [SerializeField] private PlayerAnimator playerAnimator;
        [SerializeField] private PlayerCamera playerCamera;
        [SerializeField] private PlayerSFX playerSFX;
        [SerializeField] private PlayerUI playerUI;

        public bool DebugMode => debugMode;
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

            playerInputs = new PlayerInputs();
            stateMachine = PlayerFSM.Create<IdlePS>(this);
        }

        private void FixedUpdate()
        {
            stateMachine.Update();
            playerPhysics.Update();
            playerCamera.Update();
            playerAnimator.Update();
            playerUI.Update();
        }

        public void AddCoin(int amount) => playerUI.AddCoin(amount);

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