using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace MyGame
{
    public interface IWeaponService
    {
        List<Weapon> GetWeapons(Transform shootFromPoint);
    }

    public class WeaponService : IWeaponService
    {
        public List<Weapon> GetWeapons(Transform shootFromPoint)
        {
            Weapon createTool = new CatnipPlanter(shootFromPoint);
            List<Weapon> weapons = new List<Weapon>();
            weapons.Add(new HeartGun(shootFromPoint));
            weapons.Add(new HandGun(shootFromPoint, BulletRepo.GetBullet(BulletPrefabs.SmallBullet)));

            return weapons;
        }
    }

    public class AnotherWeapoenService : IWeaponService
    {
        public List<Weapon> GetWeapons(Transform shootFromPoint)
        {
            Weapon createTool = new CatnipPlanter(shootFromPoint);
            List<Weapon> weapons = new List<Weapon>();
            weapons.Add(new HeartGun(shootFromPoint));
            weapons.Add(new HandGun(shootFromPoint, BulletRepo.GetBullet(BulletPrefabs.SmallBullet)));
            weapons.Add(new CatnipPlanter(shootFromPoint));

            return weapons;
        }
    }

}