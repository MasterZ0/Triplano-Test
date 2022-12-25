namespace TriplanoTest.Player.FSM
{
    public sealed class WalkPS : StandingPS
    {
        public override void EnterState()
        {
            base.EnterState();
            Animator.Walk();
        }

        public override void UpdateStanding()
        {
            if (!Inputs.IsMovePressed)
            {
                SwitchState<IdlePS>();
                return;
            }

            Physics.Move(Settings.WalkSpeed);
        }
    }
}