namespace TriplanoTest.StateMachine
{
    public abstract class State
    {
        protected FiniteStateMachine StateMachine { get; private set; }

        public virtual void SetupState(FiniteStateMachine controller)
        {
            StateMachine = controller;
        }

        public virtual void EnterState() { }
        public virtual void UpdateState() { }
        public virtual void ExitState() { }
        public virtual void DrawGizmos() { }

        protected void SwitchState<T>() where T : State, new()
        {
            StateMachine.SwitchState<T>();
        }

        protected bool LastStateIs<T>() where T : State
        {
            return StateMachine.LastState is T;
        }

        protected bool NextStateIs<T>() where T : State
        {
            return StateMachine.NextState is T;
        }
    }

    public abstract class State<T> : State where T : FiniteStateMachine
    {
        protected new T StateMachine { get; private set; }

        public override void SetupState(FiniteStateMachine controller)
        {
            StateMachine = controller as T;
        }
    }
}