using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

namespace BehaviourTree
{
    public class GuardBehaviourTree : Tree
    {
        public UnityEngine.Transform[] waypoints;

        public static float speed = 2f;
        public static float fovRange = 6f;
        public static float attackRange = 1f;

        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CheckPlayerInSight(transform),
                    new Attack(transform),
                }),
                new Sequence(new List<Node>
                {
                    new CheckPlayerInSight(transform),
                    new TaskGoToTarget(transform),
                }),
                new Patrol(transform, waypoints),
            });

            return root;
        }
    }
}