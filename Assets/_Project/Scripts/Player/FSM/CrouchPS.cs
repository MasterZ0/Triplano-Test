namespace TriplanoTest.Player.FSM
{
    public sealed class CrouchPS : CrouchSPS
    {
        public override void EnterState()
        {
            base.EnterState();
            Animator.Crounch();
        }

        public override void UpdateCrouch()
        {
            if (Inputs.IsMovePressed)
            {
                SwitchState<CrouchWalkPS>();
            }
        }
    }
}