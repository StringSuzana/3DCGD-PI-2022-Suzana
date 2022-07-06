using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime;
    private float maxLifeTime = 20;

    public Rigidbody bulletRigidBody;
    public GameObject particleEffect;

    void Start()
    {

    }

    void Update()
    {
        maxLifeTime -= Time.deltaTime;
        Debug.Log("lifeTime " + lifeTime);

        if (maxLifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player")
        {
            Debug.Log("======================================================= ");
            var go = Instantiate(particleEffect, transform.position, transform.rotation);
            Debug.Log("Collider other: " + other.name);
            Debug.Log("Particles " + go.name);
            Destroy(gameObject);
        }
    }
}
