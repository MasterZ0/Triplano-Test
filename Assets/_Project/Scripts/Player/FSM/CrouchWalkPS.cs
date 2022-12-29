namespace TriplanoTest.Player.FSM
{
    public sealed class CrouchWalkPS : CrouchSPS
    {
        public override void EnterState()
        {
            base.EnterState();
            Animator.CrounchWalk();
        }

        public override void UpdateCrouch()
        {
            if (!Inputs.IsMovePressed)
            {
                SwitchState<CrouchPS>();
                return;
            }

            Physics.Move(Data.CrouchSpeed);
        }
    }
}