using UnityEngine;

namespace HSM
{
    public class NpcPatrolState : NpcBaseState
    {
        public NpcPatrolState(NpcStateMachine context, NpcStateFactory npcStateFactory)
            : base(context, npcStateFactory)
        {
            _isRootState = true;
        }

        public override void EnterState()
        {
            Debug.Log("Enter  Patrol State");
            _context.Agent.isStopped = false;
            _context.HasNextWayPoint = false;
            GoToNextWayPoint();
        }

        public override void UpdateState()
        {
            GoToNextWayPoint();
        }

        public override void ExitState()
        {
            _context.Agent.isStopped = true;
            _context.Animator.SetBool(_context.Walk, false);
            Debug.Log("Exit Patrol State");
        }

        public override void InitSubState()
        {
        }

        public override void CheckSwitchStates()
        {
            if (_context.IsPlayerInSight)
            {
                SwitchState(_npcStateFactory.PlayerInSight());
            }
        }

        private void GoToNextWayPoint()
        {
            if (_context.HasNextWayPoint) return;
            Debug.Log($"[old]CurrentWayPoint {_context.CurrentWayPoint}");
      
            _context.CurrentWayPoint += 1;
            _context.CurrentWayPoint %= _context.WayPoints.Length;
            _context.NextWalkPoint = _context.WayPoints[_context.CurrentWayPoint].transform.position;
            Debug.Log($"[new]CurrentWayPoint {_context.CurrentWayPoint}");
            Debug.Log($"NextWalkPoint {_context.CurrentWayPoint}");
            _context.Agent.SetDestination(_context.NextWalkPoint);
            _context.transform.LookAt(_context.NextWalkPoint);
            _context.Animator.SetBool(_context.Walk, true);


            _context.HasNextWayPoint = true;
        }
    }
}