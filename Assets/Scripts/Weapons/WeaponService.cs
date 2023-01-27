using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace MyGame
{
    public interface IWeaponService
    {
        List<Weapon> GetWeapons(Transform shootFromPoint);
    }

    public class FirstLevelWeaponService : IWeaponService
    {
        public List<Weapon> GetWeapons(Transform shootFromPoint)
        {
            Weapon bagWeapon = new BagThrower(shootFromPoint);
            List<Weapon> weapons = new List<Weapon>();
            weapons.Add(new HeartGun(shootFromPoint));
            weapons.Add(new HandGun(shootFromPoint, BulletRepo.GetBullet(BulletPrefabs.Hand)));

            return weapons;
        }
    }

    public class ThridLevelWeaponService : IWeaponService
    {
        public List<Weapon> GetWeapons(Transform shootFromPoint)
        {
            Weapon createTool = new BagThrower(shootFromPoint);
            List<Weapon> weapons = new List<Weapon>();
            weapons.Add(new HeartGun(shootFromPoint));
            weapons.Add(new HandGun(shootFromPoint, BulletRepo.GetBullet(BulletPrefabs.Hand)));
            weapons.Add(new BagThrower(shootFromPoint));

            return weapons;
        }
    }

}