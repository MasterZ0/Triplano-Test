using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TriplanoTest.Inputs
{
    /// <summary>
    /// Send UI Inputs events
    /// </summary>
    public class UIInputs : BaseInput
    {
        public event Action OnPause = delegate { };
        public event Action OnSubmit = delegate { };
        public event Action OnCancel = delegate { };
        public event Action<Vector2> OnMove = delegate { };

        public UIInputs(bool enable = true) : base(enable)
        {
            controls.UI.Pause.started += OnPressPause;
            controls.UI.Submit.started += OnPressSubmit;
            controls.UI.Cancel.started += OnPressCancel;
            controls.UI.Move.started += OnPressMove;
        }

        private void OnPressMove(InputAction.CallbackContext obj)
        {
            Vector2 direction = obj.ReadValue<Vector2>();
            OnMove(direction);
        }

        private void OnPressSubmit(InputAction.CallbackContext _) => OnSubmit();
        private void OnPressCancel(InputAction.CallbackContext _) => OnCancel();
        private void OnPressPause(InputAction.CallbackContext _) => OnPause();
    }
}