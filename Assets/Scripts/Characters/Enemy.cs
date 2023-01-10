using System.Collections;
using MyGame;
using UnityEngine;
using UnityEngine.AI;
using Weapons;

namespace Characters
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        private float _stunForSeconds;
        private IPlayer _iPlayer;

        public float health = 50;
        public float enemyDamageAmount = 15;
        public int lives = 3;
        public GameObject attackTarget;
        public GameObject particleDieEffect;

        public Animator animator;
        private static readonly int DeadTriggerAnim = Animator.StringToHash("dead");
        private static readonly int AttackTriggerAnim = Animator.StringToHash("attack");
        private static readonly int SpeedFloatAnim = Animator.StringToHash("speed");

        public AudioSource AttackAudioSource;

        public NavMeshAgent agent;

        public Transform player;

        public LayerMask groundLayer, playerLayer;

        //Patroling
        public Vector3 walkPoint;
        bool walkPointSet;
        public float walkPointRange;

        //Attacking
        public float timeBetweenAttacks;
        bool alreadyAttacked;

        //States
        public float sightRange, attackRange;
        public bool playerInSightRange, playerInAttackRange;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();

            _iPlayer = player.parent.GetComponent<IPlayer>();
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
            if (!walkPointSet) FindRandomWalkPoint();

            if (walkPointSet)
                if (agent.SetDestination(walkPoint))
                {
                    animator.SetFloat("speed", 1);
                }

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 1f)
            {
                walkPointSet = false;
                animator.SetFloat("speed", 0);
            }

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

        public void AttackTarget()
        {
            transform.LookAt(attackTarget.transform.position);
            //Stop Enemy from moving
            agent.SetDestination(transform.position);

            if (!alreadyAttacked)
            {
                Debug.Log("AttackTarget with 15 damage.");
                AttackAudioSource.Play();
                StartCoroutine(_iPlayer.TakeDamage(enemyDamageAmount));
                animator.SetTrigger(AttackTriggerAnim);
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

            animator.enabled = false;
            agent.SetDestination(transform.position);
            agent.isStopped = true;
        }

        public void Stop()
        {
            Debug.Log("Stop attacking");
            this._stunForSeconds = 200;
            animator.enabled = false;
            agent.isStopped = true;
        }

        private void Unstun()
        {
            animator.enabled = true;
            agent.isStopped = false;
        }


        public void FollowTarget()
        {
            Debug.Log("FollowTarget ");
            agent.SetDestination(attackTarget.transform.position);
            animator.SetFloat(SpeedFloatAnim, 1);
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
            animator.SetTrigger(DeadTriggerAnim);

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