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
            Interact();
            HideNpcAlertCanvas();
            ShowInstructionsCanvas();
        }

        public override void UpdateState()
        {
            if (HintManager.Instance.IsDone)
            {
                StopTalkAnimation();
            }

            CheckSwitchStates();
        }

        public override void ExitState()
        {
            EndInteraction();
        }

        public override void CheckSwitchStates()
        {
            if (_context.IsInInteractRange && _context.IsTalking == false)
            {
                SwitchState(_npcStateFactory.Idle());
            }
            else if (_context.IsInInteractRange == false)
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
            HintManager.Instance.EndDialogue();
            StopTalkAnimation();
        }


        public void Interact()
        {
            _context.transform.LookAt(_context.Player);
            HintManager.Instance.StartHint(_context.HintText);
            PlayTalkAnimation();
            PlayInteractSound();
        }

        private void PlayTalkAnimation()
        {
            _context.Animator.SetBool(_context.Talk, true);
        }

        private void StopTalkAnimation()
        {
            _context.Animator.SetBool(_context.Talk, false);
        }
        private void ShowInstructionsCanvas()
        {
            _context.InteractionInstructionsCanvas.gameObject.SetActive(true);
        }

        private void HideNpcAlertCanvas()
        {
            _context.NpcAlertCanvas.gameObject.SetActive(false);
        }
    }
}