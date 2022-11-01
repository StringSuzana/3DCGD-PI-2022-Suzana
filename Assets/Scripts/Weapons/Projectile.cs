using UnityEngine;

namespace MyGame
{
    public enum ProjectileType
    {
        damage, attract, stun
    }
    public class Projectile : MonoBehaviour
    {
        public ProjectileType projectileType;
        public int damage;

    }
}