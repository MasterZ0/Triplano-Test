namespace TriplanoTest.Player.FSM
{
    public sealed class CrouchPS : PlayerState
    {
        public override void EnterState()
        {
            Animator.Crounch();
        }

        public override void UpdateState()
        {
            if (!Physics.CheckGround())
            {
                SwitchState<AirPS>();
                return;
            }

            if (!Inputs.IsCrouchPressed)
            {
                SwitchState<IdlePS>();
            }
        }
    }
}