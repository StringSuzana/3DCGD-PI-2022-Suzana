namespace HSM
{
    /**Creates concrete states from context*/
    public class NpcStateFactory
    {
        private NpcStateMachine _context;

        public NpcStateFactory(NpcStateMachine currentContext)
        {
            _context = currentContext;
        }

        public NpcBaseState Idle()
        {
            return new NpcIdleState(_context, this);
        }
        public NpcBaseState Patrol()
        {
            return new NpcPatrolState(_context, this);
        }
        public NpcBaseState Talk()
        {
            return new NpcTalkState(_context, this);
        }
        public NpcBaseState Follow()
        {
            return new NpcFollowState(_context, this);
        }
        public NpcBaseState PlayerInSight()
        {
            return new NpcPlayerInSightState(_context, this);
        }

    }
}