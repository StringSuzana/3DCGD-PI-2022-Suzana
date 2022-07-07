using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    public float health = 50;
    public int lives = 3;
    public GameObject target;
    private float stunForSeconds;
    public GameObject particleDieEffect;

    public void StopMovingForSeconds(float damageAmount)
    {
        this.stunForSeconds = damageAmount;
        Debug.Log("StopMovingForSeconds: " + damageAmount);
        var anim = GetComponent<Animator>();
        anim.enabled = false;
    }
    private void Unstun()
    {
        var anim = GetComponent<Animator>();
        anim.enabled = true;
    }
    void Update()
    {
        stunForSeconds -= Time.deltaTime;
        if (stunForSeconds <= 0)
        {
            Unstun();
        }
    }
    void IEnemy.AttackTarget(PlayerController player)
    {
        Debug.Log("AttackTarget ");
    }



    void IEnemy.FollowTarget(Transform targetTransform)
    {
        Debug.Log("FollowTarget ");

    }

    float IEnemy.GetHealth()
    {
        Debug.Log("GetHealth " + health);
        return health;
    }


    void IEnemy.TakeDamage(float damageAmount)
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

        var anim = GetComponent<Animator>();
        anim.SetTrigger("dead");

        yield return new WaitForSecondsRealtime(2f);

        var pe = Instantiate(particleDieEffect, transform.position, transform.rotation);
        Destroy(gameObject);

   
    }


}
