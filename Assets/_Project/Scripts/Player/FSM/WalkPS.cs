namespace TriplanoTest.Player.FSM
{
    public sealed class WalkPS : StandingSPS
    {
        public override void EnterState()
        {
            base.EnterState();
            Animator.Run();
        }

        public override void UpdateStanding()
        {
            if (!Inputs.IsMovePressed)
            {
                SwitchState<IdlePS>();
                return;
            }

            Physics.Move(Data.WalkSpeed);
        }
    }
}