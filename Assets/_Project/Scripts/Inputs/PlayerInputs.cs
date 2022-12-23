using System;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace TriplanoTest.Inputs
{
    public class PlayerInputs : BaseInput
    {
        public event Action<Vector2> OnMoveCamera;
        public event Action OnJumpReleased;
        public event Action OnJumpPressed;
        public event Action OnInteractPressed;
        public event Action OnInteractReleased;
        public event Action OnPrimarySkillPressed;
        public event Action OnPrimarySkillReleased;
        public event Action OnSecondarySkillPressed;
        public event Action OnSecondarySkillReleased;

        public bool MovePressed => Move != Vector2.zero;
        public bool JumpPressed { get; private set; }
        public bool InteractPressed { get; private set; }
        public bool PrimarySkillPressed { get; private set; }
        public bool SecondarySkillPressed { get; private set; }

        public Vector2 Move => controls.Player.Move.ReadValue<Vector2>();
        public Vector2 Look => controls.Player.Look.ReadValue<Vector2>();

        public PlayerInputs(bool enable = true) : base(enable)
        {
            controls.Player.Jump.started += OnJumpDown;
            controls.Player.Jump.canceled += OnJumpUp;
            controls.Player.Interact.started += OnInteractDown;
            controls.Player.Interact.canceled += OnInteractUp;
            controls.Player.Look.started += OnLook;
        }

        private void OnLook(CallbackContext ctx) => OnMoveCamera(ctx.ReadValue<Vector2>());

        private void OnJumpDown(CallbackContext _)
        {
            JumpPressed = true;
            OnJumpPressed?.Invoke();
        }

        private void OnJumpUp(CallbackContext _)
        {
            JumpPressed = false;
            OnJumpReleased?.Invoke();
        }

        private void OnInteractDown(CallbackContext _)
        {
            InteractPressed = true;
            OnInteractPressed?.Invoke();
        }

        private void OnInteractUp(CallbackContext _)
        {
            InteractPressed = false;
            OnInteractReleased?.Invoke();
        }
    }
}