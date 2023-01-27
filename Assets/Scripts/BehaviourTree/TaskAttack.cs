using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace BehaviourTree
{
    public class TaskAttack : Node
    {
        private Animator _animator;
        
        private IFpsPlayer _fpsPlayer;

        private float _attackTime = 1f;
        private float _attackCounter = 0f;
        private float _damageAmount = 10f;
        private static readonly int Attacking = Animator.StringToHash("Attacking");
        private static readonly int Walking = Animator.StringToHash("Walking");

        public TaskAttack(Transform transform, Transform attackPoint)
        {
            _animator = transform.GetComponent<Animator>();
            _fpsPlayer = attackPoint.parent.GetComponent<IFpsPlayer>();
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

            _attackCounter += Time.deltaTime;
            if (_attackCounter >= _attackTime)
            {
                bool enemyIsDead = _fpsPlayer.TakeDamage(_damageAmount);
                if (enemyIsDead)
                {
                    ClearData("target");
                    _animator.SetBool(Attacking, false);
                    _animator.SetBool(Walking,   true);
                }
                else
                {
                    _attackCounter = 0f;
                }
            }

            state = NodeState.RUNNING;
            return state;
        }

    }
}