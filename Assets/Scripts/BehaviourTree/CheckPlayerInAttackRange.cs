using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class CheckPlayerInAttackRange : Node
    {
        private Transform _transform;
        private Animator _animator;
        private static readonly int Attacking = Animator.StringToHash("Attacking");
        private static readonly int Walking = Animator.StringToHash("Walking");

        public CheckPlayerInAttackRange(Transform transform)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                state = NodeState.FAILURE;
                return state;
            }

            Transform target = (Transform)t;
            if (Vector3.Distance(_transform.position, target.position) <= GuardBehaviourTree.AttackRange)
            {
                Debug.Log("In attack range");
                _animator.SetBool(Attacking, true);
                _animator.SetBool(Walking,   false);

                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}