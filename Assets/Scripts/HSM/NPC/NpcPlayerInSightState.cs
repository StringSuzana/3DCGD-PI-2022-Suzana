using UnityEngine;

namespace HSM
{
    public class NpcPlayerInSightState : NpcBaseState
    {
        public NpcPlayerInSightState(NpcStateMachine context, NpcStateFactory npcStateFactory)
            : base(context, npcStateFactory)
        {
            _isRootState = true;
            InitSubState();
        }

        public override void EnterState()
        {
            Debug.Log("Enter PlayerInSight State");
            ShowInstructionsCanvas();
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }

        public override void ExitState()
        {
            Debug.Log("Exit PlayerInSight State");
            HideInstructionsCanvas();
        }

        public override void CheckSwitchStates()
        {
            if (_context.IsPlayerInSight == false)
            {
                SwitchState(_npcStateFactory.Patrol());
            }
        }

        //SEALED!!
        public sealed override void InitSubState()
        {
            Debug.Log("Init Sub states [NpcPlayerInSight]");
            Debug.Log($"IsTalking{_context.IsTalking}");
            Debug.Log($"IsFollowingPlayer{_context.IsFollowingPlayer}");
            Debug.Log($"IsIdle{_context.IsIdle}");
            if (_context.IsTalking)
            {
                SetSubState(_npcStateFactory.Talk());
            }

            else if (_context.IsFollowingPlayer)
            {
                SetSubState(_npcStateFactory.Follow());
            }

            else if (_context.IsIdle)
            {
                SetSubState(_npcStateFactory.Idle());
            }
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