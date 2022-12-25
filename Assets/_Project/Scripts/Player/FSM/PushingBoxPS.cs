using TriplanoTest.Gameplay;
using UnityEngine;

namespace TriplanoTest.Player.FSM
{
    public class PushingBoxPS : PlayerState
    {
        private bool preparing;
        private Vector3 edgePoint;
        private Vector3 direction;
        private Vector3 holdPoint;

        private IPushable Box => Physics.CurrentPushable;

        public override void EnterState()
        {
            preparing = true;

            edgePoint = Box.GetHoldPoint(Controller.transform, out direction);
            holdPoint = edgePoint + direction * Settings.PushBoxOffset;

            Animator.Walk();
            base.EnterState();
        }

        public override void UpdateState()
        {
            if (!Inputs.IsInteractPressed)
            {
                SwitchState<IdlePS>();
                return;
            }

            if (preparing)
            {
                Physics.MoveTo(holdPoint, -direction, Settings.WalkSpeed);
                Vector3 aux = holdPoint;
                aux.y = Physics.Position.y;
                bool niceDirection = Vector3.Angle(Physics.Transform.forward, -direction) < 2f;
                bool niceDistance = Vector3.Distance(aux, Physics.Position) <= 0.02f;
                preparing = !niceDirection || !niceDistance;
            }
            else
            {
                if (Inputs.IsMovePressed)
                {
                    Animator.Walk();
                    Physics.MovePush();
                }
                else
                {
                    Animator.Idle();
                }
            }
        }

        public override void DrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(edgePoint, 0.2f);
            Gizmos.DrawSphere(holdPoint, 0.2f);
        }
    }
}