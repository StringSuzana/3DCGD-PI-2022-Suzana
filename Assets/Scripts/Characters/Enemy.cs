using System;
using System.Collections;
using MyGame;
using UnityEngine;
using UnityEngine.AI;
using Weapons;
using Random = UnityEngine.Random;

namespace Characters
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        private float _stunForSeconds;
        private IFpsPlayer _iFpsPlayer;

        [SerializeField] private float health = 50;
        [SerializeField] private float enemyDamageAmount = 15;
        [SerializeField] private int lives = 3;
        [SerializeField] private GameObject attackTarget;
        [SerializeField] private GameObject particleDieEffect;

        private Animator _animator;
        private static readonly int DeadTriggerAnim = Animator.StringToHash("dead");
        private static readonly int AttackTriggerAnim = Animator.StringToHash("attack");
        private static readonly int SpeedFloatAnim = Animator.StringToHash("speed");

        [SerializeField] private AudioSource attackAudioSource;

        [SerializeField] private NavMeshAgent agent;

        [SerializeField] private Transform player;

        [SerializeField] private LayerMask groundLayer, playerLayer;

        //Patroling
        [SerializeField] private Vector3 walkPoint;
        [SerializeField] private GameObject[] wayPoints;

        bool walkPointSet;
        [SerializeField] private float walkPointRange;

        //Attacking
        [SerializeField] private float timeBetweenAttacks;
        bool alreadyAttacked = false;

        //States
        [SerializeField] private float sightRange;
        [SerializeField] private float attackRange;

        [SerializeField] private bool playerInSightRange;
        [SerializeField] private bool playerInAttackRange;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();

            _iFpsPlayer = player.parent.GetComponent<IFpsPlayer>();
        }

        private void Start()
        {
            GoToRandomWayPoint();
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Projectile>() != null)
            {
                Debug.Log("PROJECTILE");
                var projectile = collision.gameObject.GetComponent<Projectile>();
                switch (projectile.projectileType)
                {
                    case ProjectileType.Attract:
                        //go to target
                        break;
                    case ProjectileType.Damage:
                        Debug.Log("Enemy takes damage");
                        TakeDamage(projectile.damage);
                        break;
                    case ProjectileType.Stun:
                        Debug.Log("Enemy is stunned");
                        Stun(projectile.damage);
                        break;
                }
            }
        }

        private void Update()
        {
            _stunForSeconds -= Time.deltaTime;
            if (_stunForSeconds <= 0)
            {
                Unstun();
                _animator.SetFloat(SpeedFloatAnim, agent.velocity.magnitude);
            }

            //Check for InSight and InAttack range
            playerInSightRange = Physics.CheckSphere(transform.position,  sightRange,  playerLayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

            if (!playerInSightRange && !playerInAttackRange) Patrol();
            if (playerInSightRange && !playerInAttackRange) FollowTarget();
            if (playerInAttackRange && playerInSightRange) AttackTarget();
        }

        private void Patrol()
        {
            if (agent.remainingDistance < 0.5) GoToRandomWayPoint();

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            transform.LookAt(walkPoint);
        }

        private void FindRandomWalkPoint()
        {
            //Calculate random point in range
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y,
                transform.position.z + randomZ);

            if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
                walkPointSet = true;
        }

        private void GoToRandomWayPoint()
        {
            int randomIndex = Random.Range(0, wayPoints.Length);
            walkPoint = wayPoints[randomIndex].transform.position;
            agent.SetDestination(walkPoint);
        }

        public void AttackTarget()
        {
            transform.LookAt(attackTarget.transform.position);
            //Stop Enemy from moving
            agent.SetDestination(transform.position);

            if (!alreadyAttacked)
            {
                Debug.Log("AttackTarget with 15 damage.");
                attackAudioSource.Play();
                StartCoroutine(_iFpsPlayer.TakeDamage(enemyDamageAmount));
                _animator.SetTrigger(AttackTriggerAnim);
                ///End of attack

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }

        private void ResetAttack()
        {
            alreadyAttacked = false;
        }


        public void Stun(float damageAmount)
        {
            this._stunForSeconds = damageAmount;
            Debug.Log("StopMovingForSeconds: " + damageAmount);

            _animator.enabled = false;
            agent.SetDestination(transform.position);
            agent.isStopped = true;
        }

        public void Stop()
        {
            Debug.Log("Stop attacking");
            this._stunForSeconds = 200;
            _animator.enabled = false;
            agent.isStopped = true;
        }

        private void Unstun()
        {
            _animator.enabled = true;
            agent.isStopped = false;
        }


        public void FollowTarget()
        {
            Debug.Log("FollowTarget ");
            agent.SetDestination(attackTarget.transform.position);
            _animator.SetFloat(SpeedFloatAnim, 1);
        }

        public float GetHealth()
        {
            Debug.Log("Enemy health: " + health);
            return health;
        }

        public void TakeDamage(float damageAmount)
        {
            Debug.Log($"Enemy took {damageAmount} damage: ");
            health -= damageAmount;
            if (health <= 0)
            {
                StartCoroutine(Die());
            }
        }

        private IEnumerator Die()
        {
            Debug.Log("Enemy is dead.");
            _animator.SetTrigger(DeadTriggerAnim);

            yield return new WaitForSecondsRealtime(2f);

            var pe = Instantiate(particleDieEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sightRange);
        }
    }
}