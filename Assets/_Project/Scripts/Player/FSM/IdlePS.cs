namespace TriplanoTest.Player.FSM
{
    public sealed class IdlePS : StandingSPS
    {
        public override void EnterState()
        {   
            base.EnterState();
            Animator.Idle();
            Physics.SetGravityScale(Settings.GroundGravity);
        }

        public override void UpdateStanding()
        {
            if (Inputs.IsMovePressed)
            {
                SwitchState<WalkPS>();
            }
        }
    }
}