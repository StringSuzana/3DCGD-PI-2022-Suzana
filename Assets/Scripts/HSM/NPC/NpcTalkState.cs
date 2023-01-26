using UnityEngine;

namespace HSM
{
    public class NpcTalkState : NpcBaseState
    {
        public NpcTalkState(NpcStateMachine context, NpcStateFactory npcStateFactory)
            : base(context, npcStateFactory)
        {
        }

        public override void EnterState()
        {
            Debug.Log("Enter Talk State");
            Interact();
            HideNpcAlertCanvas();
            ShowInstructionsCanvas();
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }

        public override void ExitState()
        {
            Debug.Log("Exit Talk State");
            EndInteraction();
        }

        public override void CheckSwitchStates()
        {
            if (_context.IsInInteractRange && _context.IsTalking == false)
            {
                SwitchState(_npcStateFactory.Idle());
            }
            else if(_context.IsInInteractRange == false)
            {
                _context.IsTalking = false;
                SwitchState(_npcStateFactory.Follow());
            }
        }

        public override void InitSubState()
        {
        }

        private void PlayInteractSound()
        {
            AudioSource.PlayClipAtPoint(_context.InteractClip, _context.transform.position);
        }

        
        public void EndInteraction()
        {
            HideDialogueCanvas();
            _context.Animator.SetBool(_context.Talk, false);
        }

     
        public void Interact()
        {
            _context.transform.LookAt(_context.Player);
            PlayTalkAnimation();
            PlayInteractSound();
            ShowDialogueCanvas();
        }

        private void PlayTalkAnimation()
        {
            _context.Animator.SetBool(_context.Talk, true);
        }
        private void ShowInstructionsCanvas()
        {
            _context.InteractionInstructionsCanvas.gameObject.SetActive(true);
        }

        private void HideNpcAlertCanvas()
        {
            _context.NpcAlertCanvas.gameObject.SetActive(false);
        }
        private void ShowDialogueCanvas()
        {
            _context.DialogueCanvas.gameObject.SetActive(true);
        }
        private void HideDialogueCanvas()
        {
            _context.DialogueCanvas.gameObject.SetActive(false);
        }


    }
}