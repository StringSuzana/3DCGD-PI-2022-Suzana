using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace MyGame
{
    public class Enemy : MonoBehaviour, IEnemy
    {

        private float stunForSeconds;
        private IPlayer iPlayer;

        public float health = 50;
        public float dealthDamage = 15;
        public int lives = 3;
        public GameObject attackTarget;
        public GameObject particleDieEffect;

        public Animator anim;

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
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();

            iPlayer = player.parent.GetComponent<IPlayer>();
        }
        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Projectile>() != null)
            {
                Debug.Log("PROJECTILE");
                var projectile = collision.gameObject.GetComponent<Projectile>();
                switch (projectile.projectileType)
                {
                    case ProjectileType.attract:
                        //go to target
                        break;
                    case ProjectileType.damage:
                        Debug.Log("Enemy takes damage");
                        TakeDamage(projectile.damage);
                        break;
                    case ProjectileType.stun:
                        Debug.Log("Enemy is stunned");
                        Stun(projectile.damage);
                        break;
                }
            }
        }
        private void Update()
        {
            stunForSeconds -= Time.deltaTime;
            if (stunForSeconds <= 0)
            {
                Unstun();
            }

            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) FollowTarget();
            if (playerInAttackRange && playerInSightRange) AttackTarget();
        }
        private void Patroling()
        {
            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
                if (agent.SetDestination(walkPoint))
                    anim.SetFloat("speed", 1);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 1f)
            {
                walkPointSet = false;
                anim.SetFloat("speed", 0);
            }
        }
        private void SearchWalkPoint()
        {
            //Calculate random point in range
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

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
                StartCoroutine(iPlayer.TakeDamage(dealthDamage));
                anim.SetTrigger("attack");
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
            this.stunForSeconds = damageAmount;
            Debug.Log("StopMovingForSeconds: " + damageAmount);

            anim.enabled = false;
            agent.SetDestination(transform.position);
            agent.isStopped = true;
        }
        private void Unstun()
        {
            anim.enabled = true;
            agent.isStopped = false;
        }



        public void FollowTarget()
        {
            Debug.Log("FollowTarget ");
            agent.SetDestination(attackTarget.transform.position);
            anim.SetFloat("speed", 1);
        }

        public float GetHealth()
        {
            Debug.Log("GetHealth " + health);
            return health;
        }

        public void TakeDamage(float damageAmount)
        {
            Debug.Log("Take damage " + damageAmount);
            health -= damageAmount;
            if (health <= 0)
            {
                StartCoroutine(Die());
            }
        }

        private IEnumerator Die()
        {
            Debug.Log("Enemy dead.");
            anim.SetTrigger("dead");

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
