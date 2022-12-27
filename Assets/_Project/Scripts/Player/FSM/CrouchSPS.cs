namespace TriplanoTest.Player.FSM
{
    public abstract class CrouchSPS : PlayerState
    {
        public override void EnterState()
        {
            Physics.UseCrouchCollider();
        }

        public override void ExitState()
        {
            Physics.UseStandCollider();
        }

        public sealed override void UpdateState()
        {
            if (!Physics.CheckGround())
            {
                SwitchState<AirPS>();
                return;
            }

            if (!Inputs.IsCrouchPressed && Physics.CanStand())
            {
                SwitchState<IdlePS>();
            }

            UpdateCrouch();
        }

        public virtual void UpdateCrouch()
        {
            if (Inputs.IsMovePressed)
            {
                SwitchState<WalkPS>();
            }
        }
    }
}