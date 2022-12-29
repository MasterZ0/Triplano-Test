using TriplanoTest.Data;
using TriplanoTest.Inputs;
using TriplanoTest.StateMachine;

namespace TriplanoTest.Player.FSM
{
    public abstract class PlayerState : State<PlayerFSM>
    {
        protected PlayerController Controller => StateMachine.Controller;
        protected PlayerData Data => Controller.Data;
        protected PlayerPhysics Physics => Controller.Physics;
        protected PlayerAnimator Animator => Controller.Animator;
        protected PlayerCamera Camera => Controller.Camera;
        protected PlayerSFX SFX => Controller.SFX;
        protected PlayerInputs Inputs => Controller.Inputs;
    }
}