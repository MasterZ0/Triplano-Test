using System;
using TriplanoTest.Inputs;
using UnityEngine;

namespace TriplanoTest.Player
{
    [Serializable]
    public sealed class PlayerDebug : PlayerControllerComponent
    {
        [SerializeField] private bool logStates;

        private DebugInputs debugInputs;
        private bool playerInvisible;

        public bool LogStates => logStates;

        public override void Init(PlayerController controller)
        {
            base.Init(controller);

            debugInputs = new DebugInputs();
            debugInputs.OnInvisible += OnInvisible;
        }

        public void Destroy()
        {
            debugInputs.Dispose();
        }

        private void OnInvisible()
        {
            playerInvisible = !playerInvisible;
            if (playerInvisible)
            {
                Controller.Animator.HidePlayer();
                Controller.Physics.HidePlayer();
            }
            else
            {
                Controller.Animator.ShowPlayer();
                Controller.Physics.ShowPlayer();
            }
        }
    }
}