using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceProvider
{
    public static IWeaponService WeaponService()
    {
        return new WeaponService();
    }
    public static IWeaponService MockWeaponService()
    {
        return new MockWeapoenService();
    }
}