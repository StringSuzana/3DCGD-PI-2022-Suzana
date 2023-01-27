using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

namespace BehaviourTree
{
    public class GuardBehaviourTree : Tree
    {
        public UnityEngine.Transform[] waypoints;
        public UnityEngine.Transform attackPoint;

        public static float Speed = 2f;
        public static float SightRange = 6f;
        public static float AttackRange = 2f;

        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CheckPlayerInAttackRange(transform),
                    new TaskAttack(transform, attackPoint),
                }),
                new Sequence(new List<Node>
                {
                    new CheckPlayerInSight(transform),
                    new TaskGoToTarget(transform),
                }),
                new TaskPatrol(transform, waypoints),
            });

            return root;
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, SightRange);
        }
    }
}