namespace TriplanoTest.Player.FSM
{
    public class IdlePS : PlayerState
    {
        public override void EnterState()
        {   
            Animator.Idle(StateMachine.OverrideIdle);
            StateMachine.OverrideIdle = string.Empty;

        }
    }
}