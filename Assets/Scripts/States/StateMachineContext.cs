using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.States
{
    public interface IStateMachineState
    {
        void SetParent(StateMachineContext parent);
        List<StateMachineTransition> GetValidTransitions();
        void Update();
        void OnStateEnter();
        void OnStateExit();
        void SetupTransitions();
    }
    public class StateMachineContext : IStateMachineState
    {
        public Dictionary<Type, IStateMachineState> states = new Dictionary<Type, IStateMachineState>();

        public IStateMachineState currentState;
        public IStateMachineState nextState;

        public List<StateMachineTransition> transitions = new List<StateMachineTransition>();

        public StateMachineContext parentStateMachine;
        public void InitializeMachine(Type initialState)
        {
            states.TryGetValue(initialState, out currentState);
            currentState.OnStateEnter();

        }
        public virtual void Update()
        {
            if (nextState != null)
            {
                ProcessTransitionForState(nextState);
            }
            ProcessTransitionForState(currentState);
            currentState.Update();

        }
        private void ProcessTransitionForState(IStateMachineState nextState)
        {
            foreach (var transition in nextState.GetValidTransitions())
            {
                if (transition.Evaluate())
                {
                    ChangeState(states[transition.TargetState]);
                    return;
                }
            }
        }
        private void ChangeState(IStateMachineState nextState)
        {
            if (nextState == currentState) return;

            currentState.OnStateExit();
            nextState.OnStateEnter();
            currentState = nextState;
        }
        public List<StateMachineTransition> GetValidTransitions()
        {
            throw new System.NotImplementedException();
        }

        public void SetParent(StateMachineContext parent)
        {
            parentStateMachine = parent;
        }

        public virtual void OnStateEnter()
        {
            currentState.OnStateEnter();
        }

        public virtual void OnStateExit()
        {
            currentState.OnStateExit();
        }

        public virtual void SetupTransitions() { }

        public void AddState(IStateMachineState state)
        {
            states[state.GetType()] = state;
            state.SetParent(this);
            state.SetupTransitions();
        }
        public void AddNextState(IStateMachineState state)
        {
            nextState = state;
            nextState.SetParent(this);
            nextState.SetupTransitions();
        }

    }
}