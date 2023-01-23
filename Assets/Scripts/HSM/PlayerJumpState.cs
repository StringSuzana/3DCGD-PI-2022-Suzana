using System.Collections;
using UnityEngine;

namespace HSM
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerStateMachine context, PlayerStateFactory playerStateFactory)
            : base(context, playerStateFactory)
        {
        }

        public override void EnterState()
        {
            Debug.Log("Jump state");
            _context.StartCoroutine(JumpAction());
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }

        public override void ExitState()
        {
        }

        public override void CheckSwitchStates()
        {
            if (_context.IsGrounded)
            {
                SwitchState(_playerStateFactory.Grounded());
            }
        }

        public override void InitSubState()
        {
        }

        protected IEnumerator JumpAction()
        {
            _context.Animator.SetTrigger(_context.JumpTriggerAnim);
            _context.IsJumping = true;

            _context.velocity.y = _context.JumpHeight /2;
            yield return new WaitForSeconds(0.5f);

            _context.velocity.y = _context.JumpHeight;
            PlayJumpSoundFx();

            yield return new WaitForSeconds(2.5f);
            _context.IsJumping = false;

        }

        private void PlayJumpSoundFx()
        {
            if (_context.SfxAudioSource.isPlaying)
                _context.SfxAudioSource.Stop();

            _context.SfxAudioSource.pitch = 0.5f;
            _context.SfxAudioSource.PlayOneShot(_context.JumpAudioClip);
        }
    }
}