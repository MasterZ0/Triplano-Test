using TriplanoTest.Data;
using TriplanoTest.Inputs;
using TriplanoTest.StateMachine;

namespace TriplanoTest.Player.FSM
{
    public abstract class PlayerState : State<PlayerFSM>
    {
        private PlayerController Controller => StateMachine.Controller;
        protected PlayerData Settings => GameData.Player;

        // Player Components
        protected PlayerPhysics Physics => Controller.Physics;
        protected PlayerAnimator Animator => Controller.Animator;
        protected PlayerInputs Input => Controller.Inputs;
    }
}