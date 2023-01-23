namespace HSM
{
    /**NpcBaseState is an abstract state*/
    public abstract class PlayerBaseState
    {
        protected PlayerStateMachine _context;
        protected PlayerStateFactory _playerStateFactory;

        protected PlayerBaseState(PlayerStateMachine context, PlayerStateFactory playerStateFactory)
        {
            _context = context;
            _playerStateFactory = playerStateFactory;
        }

        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
        public abstract void CheckSwitchStates();
        public abstract void InitSubState();

        private void UpdateStates()
        {
        }

        protected void SwitchState(PlayerBaseState newState)
        {
            //Exit from current state
            //Each state implements it's own logic
            ExitState();

            //Enter to new
            newState.EnterState();

            //Set CurrentState in PlayerStateMachine to new state
            _context.CurrentState = newState;
        }

        protected void SetSuperState()
        {
        }

        protected void SetSubState()
        {
        }
    }
}