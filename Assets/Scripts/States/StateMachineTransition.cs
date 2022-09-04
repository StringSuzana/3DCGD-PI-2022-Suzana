using System;

namespace MyGame
{
    public class StateMachineTransition
    {
        private Type targetState;
        public Type TargetState => targetState;

        private Func<bool> condition;

        public StateMachineTransition(Type targetState, Func<bool> condition)
        {
            this.targetState = targetState;
            this.condition = condition;
        }

        public bool Evaluate()
        {
            return condition();
        }
    }
}