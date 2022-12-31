using System;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace TriplanoTest.Inputs
{
    public class DebugInputs : BaseInput
    {
        public event Action OnInvisible;

        public DebugInputs(bool enable = true) : base(enable)
        {
            controls.Debug.Invisible.started += OnInvisiblePressed;
        }

        private void OnInvisiblePressed(CallbackContext _)
        {
            OnInvisible?.Invoke();
        }
    }
}