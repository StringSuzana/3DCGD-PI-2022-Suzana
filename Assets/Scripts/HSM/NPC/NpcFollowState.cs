using UnityEngine;

namespace HSM
{
    public class NpcFollowState : NpcBaseState
    {
        public NpcFollowState(NpcStateMachine context, NpcStateFactory npcStateFactory)
            : base(context, npcStateFactory)
        {
        }
        public override void EnterState()
        {
            Debug.Log("Enter Follow State");
            FollowPlayer();
        }

        public override void UpdateState()
        {
            FollowPlayer();
            CheckSwitchStates();
        }

        public override void ExitState()
        {
            Debug.Log("Exit Follow State");
            StopFollowingPlayer();
            _context.IsFollowingPlayer = false;
        }

        public override void InitSubState()
        {
        }

        public override void CheckSwitchStates()
        {
            if (_context.IsIdle)
            {
                SwitchState(_npcStateFactory.Idle());
            }
            else if (_context.IsTalking)
            {
                SwitchState(_npcStateFactory.Talk());
            }

            //  idle/talk
        }

        private void StopFollowingPlayer()
        {
            _context.Agent.isStopped = true;
            _context.transform.LookAt(_context.Player.position);
            _context.Animator.SetBool(_context.Walk, false);
        }

        private void FollowPlayer()
        {
            _context.transform.LookAt(_context.Player.position);
            _context.Agent.SetDestination(_context.transform.position);
            _context.Animator.SetBool(_context.Walk, true);

            Debug.Log("Following player.");
            //    attackAudioSource.PlayOneShot(attackAudioClip);
            //    StartCoroutine(_iFpsPlayer.TakeDamage(enemyDamageAmount));
            //    _animator.SetTrigger(AttackTriggerAnim);
            //    ///End of attack
        }
    }
}