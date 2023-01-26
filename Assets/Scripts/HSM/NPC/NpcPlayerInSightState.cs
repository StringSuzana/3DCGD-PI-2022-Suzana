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
            Debug.Log("Enter [PlayerInSight] State");
            ShowNpcAlertCanvas();
        }

        public override void UpdateState()
        {
           // ShowInstructionsCanvas();
            CheckSwitchStates();
        }

        public override void ExitState()
        {
            Debug.Log("Exit [PlayerInSight] State");
            HideAllCanvases();
            _context.IsFollowingPlayer = false;
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

            if (_context.IsInInteractRange && _context.IsTalking)
            {
                SetSubState(_npcStateFactory.Talk());
            }
            else if (_context.IsInInteractRange && _context.IsTalking == false)
            {
               SetSubState(_npcStateFactory.Idle());
            }
            else if (_context.IsInInteractRange == false)
            {
                SetSubState(_npcStateFactory.Follow());
            }
        }

        private void ShowNpcAlertCanvas()
        {
            _context.NpcAlertCanvas.gameObject.SetActive(true);
        }

        private void HideAllCanvases()
        {
            _context.NpcAlertCanvas.gameObject.SetActive(false);
            _context.InteractionInstructionsCanvas.gameObject.SetActive(false);
        }
    }
}