using System.Collections.Generic;
using MyGame;

namespace States
{
    public abstract class State : IStateMachineState
    {
        public StateMachineContext machineContext;
        public List<StateMachineTransition> transitions = new List<StateMachineTransition>();

        public void AddTransition(StateMachineTransition transition)
        {
            transitions.Add(transition);
        }

        public virtual void Update()
        {

        }
        public virtual void OnStateEnter()
        {

        }

        public virtual void OnStateExit()
        {

        }

        public abstract void SetupTransitions();

        public void SetContext(StateMachineContext ctx)
        {
            this.machineContext = ctx;
        }

        public List<StateMachineTransition> GetValidTransitions()
        {
            return transitions;
        }
    }
}