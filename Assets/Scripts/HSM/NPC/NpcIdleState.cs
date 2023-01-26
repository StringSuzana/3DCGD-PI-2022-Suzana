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
            HideInstructionsCanvas();
            _context.Animator.SetBool(_context.Idle, false);

        }

        public override void CheckSwitchStates()
        {
            if (_context.IsTalking)
            {
                SwitchState(_npcStateFactory.Talk());
            }
            else if (_context.IsInInteractRange == false)
            {
                SwitchState(_npcStateFactory.Follow());
            }
        }

        public override void InitSubState()
        {
        }

        private void BeIdle()
        {
            ShowInstructionsCanvas();
            _context.Animator.SetBool(_context.Idle, true);
        }
        private void ShowInstructionsCanvas()
        {
            _context.InteractionInstructionsCanvas.gameObject.SetActive(true);

        }

        private void HideInstructionsCanvas()
        {
            _context.InteractionInstructionsCanvas.gameObject.SetActive(false);
        }
    }
}