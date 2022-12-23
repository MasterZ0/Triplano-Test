using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace TriplanoTest.StateMachine
{
    public class FiniteStateMachine
    {
        public State CurrentState { get; protected set; }
        public State LastState { get; protected set; }
        public State NextState { get; protected set; }

        private readonly List<State> states = new();

        protected FiniteStateMachine(Type stateBaseType)
        {
            IEnumerable<Type> types = Assembly.GetAssembly(stateBaseType).GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(stateBaseType));

            foreach (Type type in types)
            {
                State state = Activator.CreateInstance(type) as State;
                state.SetupState(this);
                states.Add(state);
            }
        }

        /// <typeparam name="T">First State</typeparam>
        public static FiniteStateMachine Create<T>() where T : State, new()
        {
            Type stateType = typeof(T);
            FiniteStateMachine stateMachine = new(stateType);
            stateMachine.SetFirstState<T>();
            return stateMachine;
        }

        public void SetFirstState<T>() where T : State, new()
        {
            CurrentState = GetState<T>();
            CurrentState.EnterState();
        }

        public void Update()
        {
            CurrentState.UpdateState();
        }

        public virtual void SwitchState<T>() where T : State, new()
        {
            NextState = GetState<T>();

            CurrentState.ExitState();

            LastState = CurrentState;
            CurrentState = NextState;
            NextState = null;

            CurrentState.EnterState();
        }

        private State GetState<T>() where T : State, new()
        {
            return states.First(s => s.GetType() == typeof(T));
        }
    }
}