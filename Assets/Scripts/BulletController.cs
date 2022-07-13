using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime;
    private float maxLifeTime = 3;

    public Rigidbody bulletRigidBody;
    public GameObject particleEffect;


    void Start()
    {

    }

    void Update()
    {
        maxLifeTime -= Time.deltaTime;
      
        if (maxLifeTime <= 0)
        {
            Instantiate(particleEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
  
        if(other.tag != "Player")//don't want to collide with shooting point on player
        {
            Instantiate(particleEffect, transform.position, transform.rotation);
           
        }

    }
}
