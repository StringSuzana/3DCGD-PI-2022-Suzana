using Characters;
using UnityEngine;

namespace Weapons
{
    public class BulletController : MonoBehaviour
    {
        public float lifeTime;
        private float maxLifeTime = 10;

        public Rigidbody bulletRigidBody;
        public GameObject particleEffect;


        void Update()
        {
            //maxLifeTime -= Time.deltaTime;

            //if (maxLifeTime <= 0)
            //{
            //    Instantiate(particleEffect, transform.position, transform.rotation);
            //    Destroy(particleEffect);
            //    Destroy(gameObject);
            //}
        }

        private void OnTriggerEnter(Collider other)
        {
            //if (other.gameObject.GetComponent<Enemy>())
            //{
            //    Instantiate(particleEffect, transform.position, transform.rotation);
            //}
        }
    }
}