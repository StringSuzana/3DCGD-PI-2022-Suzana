using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class TaskPatrol : Node
    {
        private Transform _transform;
        private Animator _animator;
        private Transform[] _wayPoints;

        private int _currentWayPointIndex = 0;

        private float _waitTime = 1f;
        private float _waitCounter = 0f;
        private bool _waiting = false;
        private static readonly int Walking = Animator.StringToHash("Walking");

        public TaskPatrol(Transform transform, Transform[] wayPoints)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
            _wayPoints = wayPoints;
        }

        public override NodeState Evaluate()
        {
            if (_waiting)
            {
                _waitCounter += Time.deltaTime;
                if (_waitCounter >= _waitTime)
                {
                    _waiting = false;
                    _animator.SetBool(Walking, true);
                }
            }
            else
            {
                Transform wp = _wayPoints[_currentWayPointIndex];
                if (Vector3.Distance(_transform.position, wp.position) < 0.01f)
                {
                    _transform.position = wp.position;
                    _waitCounter = 0f;
                    _waiting = true;

                    _currentWayPointIndex = (_currentWayPointIndex + 1) % _wayPoints.Length;
                    _animator.SetBool(Walking, false);
                }
                else
                {
                    _transform.position =
                        Vector3.MoveTowards(_transform.position, wp.position, GuardBehaviourTree.Speed * Time.deltaTime);
                    _transform.LookAt(wp.position);
                }
            }


            state = NodeState.RUNNING;
            return state;
        }
    }
}