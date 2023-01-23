using UnityEngine;

namespace HSM
{
    public class PlayerGroundedState : PlayerBaseState
    {
        public PlayerGroundedState(PlayerStateMachine context, PlayerStateFactory playerStateFactory)
            : base(context, playerStateFactory)
        {

        }

        public override void EnterState()
        {
            Debug.Log("Grounded state");
            _context.velocity.y = 0;
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }

        public override void ExitState()
        {
        }

        public override void InitSubState()
        {
        }
        public override void CheckSwitchStates()
        {
            if (_context.IsJumpPressed)
            {
                SwitchState(_playerStateFactory.Jump());
            }
        }

    }
}