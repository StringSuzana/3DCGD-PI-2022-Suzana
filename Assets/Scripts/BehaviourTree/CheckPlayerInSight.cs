using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace BehaviourTree
{
    public class CheckPlayerInSight : Node
    {
        private Transform _transform;
        private Animator _animator;
        private static readonly int Walking = Animator.StringToHash("Walking");

        public CheckPlayerInSight(Transform transform)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate()
        {
            Collider[] colliders = Physics.OverlapSphere(
                _transform.position, GuardBehaviourTree.SightRange, LayerMask.GetMask("Player"));
            
            if (colliders.Length > 0)
            {
                Debug.Log("Collided with a player");

                parent.parent.SetData("target", colliders[0].transform);
                _animator.SetBool(Walking, true);
                state = NodeState.SUCCESS;
                return state;
            }

            ClearData("target");
            
            state = NodeState.FAILURE;
            return state;

        }
    }
}