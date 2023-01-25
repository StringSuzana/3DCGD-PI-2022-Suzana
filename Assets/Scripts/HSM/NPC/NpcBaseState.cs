namespace HSM
{
    /**NpcBaseState is an abstract state*/
    public abstract class NpcBaseState
    {
        protected NpcStateMachine _context;
        protected NpcStateFactory _npcStateFactory;
        protected NpcBaseState _currentSuperState;
        protected NpcBaseState _currentSubState;
        protected bool _isRootState = false;

        protected NpcBaseState(NpcStateMachine context, NpcStateFactory npcStateFactory)
        {
            _context = context;
            _npcStateFactory = npcStateFactory;
        }

        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
        public abstract void CheckSwitchStates();
        public abstract void InitSubState();

        public void UpdateStates()
        {
            UpdateState();
            if (_currentSubState != null)
            {
                _currentSubState.UpdateState();
            }
        }
        public void ExitStates()
        {
            if (_currentSubState != null)
            {
                _currentSubState.ExitState();
            }
            ExitState();
        }
        protected void SwitchState(NpcBaseState newState)
        {
            //Exit from current state
            //Each state implements it's own logic
            ExitStates();

            //Enter to new
            newState.EnterState();

            if (_isRootState)
            {
                //Set CurrentState in PlayerStateMachine to new state
                _context.CurrentState = newState;
            }
            else if (_currentSuperState != null)
            {
                _currentSuperState.SetSubState(newState);
            }
        }

        protected void SetSuperState(NpcBaseState newSuperState)
        {
            _currentSuperState = newSuperState;
        }

        protected void SetSubState(NpcBaseState newSubState)
        {
            _currentSubState = newSubState;
            newSubState.SetSuperState(this);
        }
    }
}