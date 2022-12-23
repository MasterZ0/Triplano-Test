using UnityEngine;
using TriplanoTest.Player.FSM;
using TriplanoTest.Inputs;
using TriplanoTest.Data;

namespace TriplanoTest.Player
{
    public sealed class PlayerController : MonoBehaviour 
    {
        [Header("Player Controller")]
        [SerializeField] private bool debugMode;
        [Space]
        [SerializeField] private PlayerPhysics playerPhysics;
        [SerializeField] private PlayerAnimator playerAnimator;
        [SerializeField] private PlayerCamera playerCamera;

        public bool DebugMode => debugMode;
        public PlayerData Data => GameData.Player;
        public PlayerPhysics Physics => playerPhysics;
        public PlayerAnimator Animator => playerAnimator;
        public PlayerCamera Camera => playerCamera;
        public PlayerInputs Inputs => playerInputs;

        private PlayerInputs playerInputs;
        private PlayerFSM stateMachine;

        private void Awake()
        {
            playerPhysics.Init(this);
            playerAnimator.Init(this);
            playerCamera.Init(this);

            playerInputs = new PlayerInputs();
            stateMachine = PlayerFSM.Create<IdlePS>(this);
        }

        private void FixedUpdate()
        {
            stateMachine.Update();
            playerPhysics.Update();
            playerCamera.Update();
        }
    }
}