using TriplanoTest.Shared;

namespace TriplanoTest.Player.FSM
{
    public class AirPS : PlayerState
    {
        private readonly Timer timer = new();

        private bool falling;

        private bool MinJumpApplied => timer.Counter > Settings.JumpRangeDuration.x;
        private bool MaxJumpApplied => timer.Counter >= Settings.JumpRangeDuration.y;

        public override void EnterState()
        {
            timer.Reset();
            falling = !StateMachine.IsJumping;

            if (falling)
            {
                Physics.SetGravityScale(Settings.FallingGravity);
                Animator.Falling();
            }
            else
            {
                Animator.Jump();

                //VFX.Jump();
                SFX.Jump();
                Physics.SetGravityScale(Settings.JumpGravity);
            }
        }

        public override void UpdateState()
        {
            timer.FixedTick();
            Physics.Move(Settings.WalkSpeed);

            if (StateMachine.IsJumping)
            {
                ApplyJump();
                return;
            }

            if (!falling && Physics.Velocity.y < 0) // Wait until start fall
            {
                falling = true;
                Physics.SetGravityScale(Settings.FallingGravity);
                Animator.Falling();
            }

            if (Physics.CheckGround())
            {
                SFX.Landing();
                Animator.OverrideIdleAsLanding();
                SwitchState<IdlePS>();
            }
        }

        private void ApplyJump()
        {
            Physics.Jump(Settings.JumpVelocity);

            if (MinJumpApplied && (!Inputs.IsJumpPressed || MaxJumpApplied))
            {
                StateMachine.IsJumping = false;
            }
        }
    }
}