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
            maxLifeTime -= Time.deltaTime;

            if (maxLifeTime <= 0)
            {
                Instantiate(particleEffect, transform.position, transform.rotation);
                Destroy(particleEffect);
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.tag != "Player") //don't want to collide with shooting point on player
            {
                Instantiate(particleEffect, transform.position, transform.rotation);
            }
            //Change to:
            //if (other.gameObject.GetComponent<IPlayer>() == null)
            //{
            //    Instantiate(particleEffect, transform.position, transform.rotation);
            //}
        }
    }
}