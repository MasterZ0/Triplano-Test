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
        public event Action OnCrouchPressed;
        public event Action OnCrouchReleased;

        public Vector2 Move => controls.Player.Move.ReadValue<Vector2>();
        public Vector2 Look => controls.Player.Look.ReadValue<Vector2>();

        public bool IsMovePressed => Move != Vector2.zero;
        public bool IsJumpPressed { get; private set; }
        public bool IsInteractPressed { get; private set; }
        public bool IsCrouchPressed { get; private set; }

        public PlayerInputs(bool enable = true) : base(enable)
        {
            controls.Player.Jump.started += OnJumpDown;
            controls.Player.Jump.canceled += OnJumpUp;
            controls.Player.Interact.started += OnInteractDown;
            controls.Player.Interact.canceled += OnInteractUp;
            controls.Player.Crouch.started += OnCrouchDown;
            controls.Player.Crouch.canceled += OnCrouchUp;

            controls.Player.Look.started += OnLook;
        }

        private void OnLook(CallbackContext ctx) => OnMoveCamera?.Invoke(ctx.ReadValue<Vector2>());

        private void OnJumpDown(CallbackContext _)
        {
            IsJumpPressed = true;
            OnJumpPressed?.Invoke();
        }

        private void OnJumpUp(CallbackContext _)
        {
            IsJumpPressed = false;
            OnJumpReleased?.Invoke();
        }

        private void OnInteractDown(CallbackContext _)
        {
            IsInteractPressed = true;
            OnInteractPressed?.Invoke();
        }

        private void OnInteractUp(CallbackContext _)
        {
            IsInteractPressed = false;
            OnInteractReleased?.Invoke();
        }

        private void OnCrouchDown(CallbackContext _)
        {
            IsCrouchPressed = true;
            OnCrouchPressed?.Invoke();
        }

        private void OnCrouchUp(CallbackContext _)
        {
            IsCrouchPressed = false;
            OnCrouchReleased?.Invoke();
        }
    }
}