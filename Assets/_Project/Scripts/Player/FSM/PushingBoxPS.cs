using TriplanoTest.Gameplay;
using UnityEngine;

namespace TriplanoTest.Player.FSM
{
    public class PushingBoxPS : PlayerState
    {
        private enum SubState
        {
            Preparing,
            HoldingBox,
            MovingBox,
        }

        private SubState subState;

        // Box parameters
        private Vector3 edgePoint;
        private Vector3 direction;
        private Vector3 holdPoint;

        /// <summary> Distance from box while pushing </summary>
        private Vector3 boxDistance;

        private IPushable Box => Physics.CurrentPushable;

        public override void EnterState()
        {
            subState = SubState.Preparing;

            edgePoint = Box.GetHoldPoint(Controller.transform, out direction);
            holdPoint = edgePoint + direction * Data.PushBoxOffset;

            Animator.Walk();
            Animator.SetHoldBoxWeight(1f);
            Animator.SetMoveSpeedScale(Data.MoveSpeedPushing);
        }

        public override void ExitState()
        {
            Animator.SetHoldBoxWeight(0f);
            Physics.StopInteract();
        }

        public override void UpdateState()
        {
            if (!Physics.CheckGround())
            {
                SwitchState<AirPS>();
                return;
            }

            if (!Inputs.IsInteractPressed && subState != SubState.MovingBox)
            {
                SwitchState<IdlePS>();
                return;
            }

            switch (subState)
            {
                case SubState.Preparing:
                    Preparing();
                    break;
                case SubState.HoldingBox:
                    HoldingBox();
                    break;
                case SubState.MovingBox:
                    MovingBox();
                    break;
            }
        }

        private void Preparing()
        {
            Physics.MoveTo(holdPoint, -direction, Data.WalkSpeed);

            Vector3 targetPosition = holdPoint;
            targetPosition.y = Physics.Position.y;

            bool niceDirection = Vector3.Angle(Physics.Transform.forward, -direction) < 2f;
            bool niceDistance = Vector3.Distance(targetPosition, Physics.Position) <= 0.02f;

            if (niceDirection && niceDistance)
            {
                subState = SubState.HoldingBox;
                boxDistance = Physics.Position - Box.Position;
            }
        }

        private void HoldingBox()
        {
            Vector2 inputDirection = Controller.Inputs.Move;

            if (inputDirection == Vector2.zero)
                return;

            // Try Move Box
            float angleToMove = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + Camera.CameraTarget.eulerAngles.y;
            int roundedAngle = Mathf.RoundToInt(angleToMove / 90f) * 90;

            Vector3 targetDirection = Quaternion.Euler(0f, roundedAngle, 0f) * Vector3.forward;

            if (Box.Push(targetDirection, Data.MoveSpeedPushing))
            {
                subState = SubState.MovingBox;
            }
        }

        private void MovingBox()
        {
            if (Box.IsMoving)
            {
                Physics.MoveTo(Box.Position + boxDistance, -direction, Data.MoveSpeedPushing * 2f); // Must need extra speed
            }
            else
            {
                subState = SubState.HoldingBox;
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