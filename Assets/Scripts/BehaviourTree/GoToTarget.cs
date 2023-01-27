using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class TaskGoToTarget : Node
    {
        private Transform _transform;

        public TaskGoToTarget(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
           
            Transform target = (Transform)GetData("target");

            if (Vector3.Distance(_transform.position, target.position) > 0.01f)
            {
                //Debug.Log("Go to target");
                _transform.position = Vector3.MoveTowards(
                    _transform.position, target.position, GuardBehaviourTree.Speed * Time.deltaTime);
                _transform.LookAt(target.position);
            }

            state = NodeState.RUNNING;
            return state;
        }
    }
}