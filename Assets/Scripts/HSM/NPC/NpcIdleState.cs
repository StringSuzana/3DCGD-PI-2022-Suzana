using UnityEngine;

namespace HSM
{
    public class NpcIdleState : NpcBaseState
    {
        public NpcIdleState(NpcStateMachine context, NpcStateFactory npcStateFactory)
            : base(context, npcStateFactory)
        {
        }

        public override void EnterState()
        {
            Debug.Log("Enter  Idle State");
            BeIdle();
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }

        public override void ExitState()
        {
            Debug.Log("Exit Idle State");
        }

        public override void CheckSwitchStates()
        {
            if (_context.IsTalking)
            {
                SwitchState(_npcStateFactory.Talk());
            }
        }

        public override void InitSubState()
        {
        }

        private void BeIdle()
        {
            _context.Animator.SetBool(_context.Idle, true);
        }
    }
}