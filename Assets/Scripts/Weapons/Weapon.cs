using UnityEngine;

namespace Weapons
{
    public abstract class Weapon
    {
        public string Name;
        protected Transform Origin;

        protected Weapon(Transform transform, string name)
        {
            this.Origin = transform;
            this.Name = name;
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
            if (!Physics.Raycast(Origin.position, Origin.forward, out RaycastHit hit, 100f)) return;

            //TODO: instantiate catnip GameObject
            var createdObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            createdObject.transform.position = hit.point + hit.normal * 0.5f;
            createdObject.layer = 8;
        }
    }

    public class HeartGun : Weapon
    {
        private readonly GameObject _projectile;

        public HeartGun(Transform transform) : base(transform, "Heart gun")
        {
            _projectile = BulletRepo.GetBullet(BulletPrefabs.HeartBullet);
        }

        public override void Shoot()
        {
            var createdObject = GameObject.Instantiate(_projectile);
            createdObject.transform.position = Origin.position;
            createdObject.layer = 8;

            var rb = createdObject.GetComponent<Rigidbody>();
            rb.AddForce(Origin.forward * 30f, ForceMode.Impulse);
        }
    }

    public class HandGun : Weapon
    {
        private readonly GameObject _projectile;

        public HandGun(Transform transform, GameObject projectile) : base(transform, "Hand gun")
        {
            this._projectile = projectile;
        }

        public override void Shoot()
        {
            var createdObject = GameObject.Instantiate(_projectile);
            createdObject.transform.position = Origin.position;
            createdObject.layer = 8;

            var rb = createdObject.GetComponent<Rigidbody>();
            rb.AddForce(Origin.forward * 20f, ForceMode.Impulse);
        }
    }
}