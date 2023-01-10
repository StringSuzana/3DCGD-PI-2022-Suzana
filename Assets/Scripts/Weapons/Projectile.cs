using UnityEngine;

namespace Weapons
{
    public enum ProjectileType
    {
        Damage, Attract, Stun
    }
    public class Projectile : MonoBehaviour
    {
        public ProjectileType projectileType;
        public int damage;

    }
}