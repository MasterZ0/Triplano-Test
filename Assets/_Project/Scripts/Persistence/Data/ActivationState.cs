using UnityEngine;

namespace TriplanoTest.Persistence
{
    public class ActivationState : PersistentState<bool>
    {
        [Header("Activation State")]
        [SerializeField] private bool defaultState = true;
        public override bool DefaultState => defaultState;

        public override void LoadState()
        {
            base.LoadState();
            if (!CurrentState)
            {
                gameObject.SetActive(false);
            }
        }

        public void SetState(bool state)
        {
            CurrentState = state;
        }
    }
}