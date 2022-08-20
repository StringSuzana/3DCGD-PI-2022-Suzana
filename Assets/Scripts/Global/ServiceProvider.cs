using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyGame
{
    public class ServiceProvider
    {
        public static IWeaponService WeaponService()
        {
            return new WeaponService();
        }
        public static IWeaponService MockWeaponService()
        {
            return new AnotherWeapoenService();
        }
    }
}