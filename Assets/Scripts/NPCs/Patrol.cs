using UnityEngine;
using UnityEngine.AI;

namespace NPCs
{
    public class Patrol : MonoBehaviour
    {
        [SerializeField] private GameObject[] wayPoints;
        private NavMeshAgent _agent;

        void Start()
        {
            _agent = this.GetComponent<NavMeshAgent>();
            GoToRandomWayPoint();
        }

        void Update()
        {
            if (_agent.remainingDistance < 0.5)
            {
                GoToRandomWayPoint();
            }
        }

        private void GoToRandomWayPoint()
        {
            int randomIndex = Random.Range(0, wayPoints.Length);
            _agent.SetDestination(wayPoints[randomIndex].transform.position);
        }
    }
}