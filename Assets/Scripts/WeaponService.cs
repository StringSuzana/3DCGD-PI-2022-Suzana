using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponService
{
    List<Weapon> GetWeapons(Transform shootFromPoint);
}

public class WeaponService : IWeaponService
{
    public List<Weapon> GetWeapons(Transform shootFromPoint)
    {
        Weapon pushGun = ForceGun.PushGun(shootFromPoint);
        Weapon createTool = new CatnipPlanter(shootFromPoint);
        Weapon pullGun = ForceGun.PullGun(shootFromPoint);
        List<Weapon> weapons = new List<Weapon>();
        //weapons.Add(pushGun);
        //weapons.Add(createTool);
        //weapons.Add(pullGun);
        weapons.Add(new HeartGun(shootFromPoint));
        weapons.Add(new HandGun(shootFromPoint, BulletRepo.GetBullet(BulletPrefabs.smallBullet)));

        return weapons;
    }
}

public class MockWeapoenService : IWeaponService
{
    public List<Weapon> GetWeapons(Transform shootFromPoint)
    {
        Weapon pushGun = ForceGun.PushGun(shootFromPoint);
        Weapon pullGun = ForceGun.PullGun(shootFromPoint);
        List<Weapon> weapons = new List<Weapon>();
        weapons.Add(pullGun);
        weapons.Add(pushGun);

        return weapons;
    }
}

