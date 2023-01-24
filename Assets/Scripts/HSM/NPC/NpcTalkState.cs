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
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }

        public override void ExitState()
        {
            Debug.Log("Exit Talk State");
            EndInteraction();
            _context.IsTalking = false;

        }

        public override void CheckSwitchStates()
        {
            if (_context.IsFollowingPlayer)
            {
                SwitchState(_npcStateFactory.Follow());
            }
            else if (_context.IsIdle)
            {
                SwitchState(_npcStateFactory.Idle());
            }
        }

        public override void InitSubState()
        {
        }

        private void PlayInteractSound()
        {
            AudioSource.PlayClipAtPoint(_context.InteractClip, _context.transform.position);
        }

        private void ShowDialogueCanvas()
        {
            _context.DialogueCanvas.gameObject.SetActive(true);
            _context.DialogueCanvas.enabled = true;
        }

        public void EndInteraction()
        {
            _context.DialogueCanvas.gameObject.SetActive(false);
            _context.DialogueCanvas.enabled = false;
        }

        public void Interact()
        {
            PlayInteractSound();
            ShowDialogueCanvas();
        }
    }
}