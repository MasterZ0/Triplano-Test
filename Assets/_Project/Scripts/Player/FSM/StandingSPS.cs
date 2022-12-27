namespace TriplanoTest.Player.FSM
{
    public abstract class StandingSPS : PlayerState
    {
        public override void EnterState()
        {
            Animator.Idle();

            Inputs.OnInteractPressed += OnInteractPressed;
            Inputs.OnJumpPressed += OnJumpPressed;
        }

        public override void ExitState()
        {
            Inputs.OnInteractPressed -= OnInteractPressed;
            Inputs.OnJumpPressed -= OnJumpPressed;
        }

        public sealed override void UpdateState()
        {
            if (!Physics.CheckGround())
            {
                SwitchState<AirPS>();
                return;
            }

            if (Inputs.IsCrouchPressed)
            {
                SwitchState<CrouchPS>();
                return;
            }

            UpdateStanding();
        }

        public virtual void UpdateStanding() { }

        private void OnInteractPressed()
        {
            if (Physics.TryInteract())
            {
                SwitchState<PushingBoxPS>();
            }
        }

        private void OnJumpPressed()
        {
            StateMachine.IsJumping = true;
            SwitchState<AirPS>();
        }
    }
}