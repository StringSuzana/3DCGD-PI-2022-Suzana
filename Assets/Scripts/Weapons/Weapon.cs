using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public abstract class Weapon
    {
        public string name;
        protected Transform origin;


        public Weapon(Transform transform, string name)
        {
            this.origin = transform;
            this.name = name;
        }

        public abstract void Shoot();
    }

    public class CatnipPlanter : Weapon
    {
        public CatnipPlanter(Transform transform) : base(transform, "Catnip Tool")
        {

        }

        public override void Shoot()
        {
            if (Physics.Raycast(origin.position, origin.forward, out RaycastHit hit, 100f))
            {
                //TODO: instantiate catnip GameObject
                var createdObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                createdObject.transform.position = hit.point + hit.normal * 0.5f;
                createdObject.layer = 8;
            }
        }
    }

    public class HeartGun : Weapon
    {
        GameObject projectile;
        public HeartGun(Transform transform) : base(transform, "Heart gun")
        {
            projectile = BulletRepo.GetBullet(BulletPrefabs.heartBullet);
        }

        public override void Shoot()
        {

            var createdObject = GameObject.Instantiate(projectile);
            createdObject.transform.position = origin.position;
            createdObject.layer = 8;

            var rb = createdObject.GetComponent<Rigidbody>();
            rb.AddForce(origin.forward * 30f, ForceMode.Impulse);

        }


    }

    public class HandGun : Weapon
    {
        GameObject projectile;
        public HandGun(Transform transform, GameObject projectile) : base(transform, "Hand gun")
        {
          
            this.projectile = projectile;
        }

        public override void Shoot()
        {
            var createdObject = GameObject.Instantiate(projectile);
            createdObject.transform.position = origin.position;
            createdObject.layer = 8;

            var rb = createdObject.GetComponent<Rigidbody>();
            rb.AddForce(origin.forward * 20f, ForceMode.Impulse);
        }

    }

}