using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

public class PushGun : Weapon
{
    public PushGun(Transform transform) : base(transform, "Push Gun")
    {

    }

    public override void Shoot()
    {
        var pushPower = 5f;
        //RaycastHit hit;
        if (Physics.Raycast(origin.position, origin.forward, out RaycastHit hit, 100f))
        {
            var rb = hit.transform.GetComponent<Rigidbody>();
            var direction = hit.point - origin.position;
            var force = direction.normalized * pushPower;

            if (rb != null)
            {
                rb.AddForceAtPosition(force, hit.point, ForceMode.Impulse);
            }
        }
    }


}

public class PullGun : Weapon
{
    public PullGun(Transform transform) : base(transform, "Pull Gun")
    {

    }

    public override void Shoot()
    {
        var pullPower = -5f;
        if (Physics.Raycast(origin.position, origin.forward, out RaycastHit hit, 100f))
        {
            var rb = hit.transform.GetComponent<Rigidbody>();
            var direction = hit.point - origin.position;
            var force = direction.normalized * pullPower;

            if (rb != null)
            {
                rb.AddForceAtPosition(force, hit.point, ForceMode.Impulse);
            }
        }
    }


}

public class ForceGun : Weapon
{
    float forcePower = 0f;
    public ForceGun(Transform transform, string name, float force) : base(transform, name)
    {
        this.forcePower = force;
    }

    public override void Shoot()
    {
        //RaycastHit hit;
        if (Physics.Raycast(origin.position, origin.forward, out RaycastHit hit, 100f))
        {
            Debug.Log("Did hit: " + hit.transform.name);
            //Destroy(hit.transform.gameObject);
            var rb = hit.transform.GetComponent<Rigidbody>();
            //check if there is RB
            var direction = hit.point - origin.position;
            var force = direction.normalized * forcePower;

            if (rb != null)
            {
                rb.AddForceAtPosition(force, hit.point, ForceMode.Impulse);
            }
        }
    }

    public static ForceGun PushGun(Transform transform)
    {
        return new ForceGun(transform, "push gun", 5f);
    }

    public static ForceGun PullGun(Transform transform)
    {
        return new ForceGun(transform, "pull gun", -5f);
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
        rb.AddForce(origin.forward * 6f, ForceMode.Impulse);


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
        rb.AddForce(origin.forward * 10f, ForceMode.Impulse);
    }

}