using UnityEngine;

namespace Weapons
{
    public class BulletRepo : MonoBehaviour
    {
        public static BulletRepo BulletRepoShared { get; private set; }

        [SerializeField] private GameObject smallBullet;
        [SerializeField] private GameObject bigBullet;
        [SerializeField] private GameObject heartBullet;

        private void Awake()
        {
            if (BulletRepoShared == null)
            {
                BulletRepoShared = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static GameObject GetBullet(BulletPrefabs prefab)
        {
            switch (prefab)
            {
                case BulletPrefabs.SmallBullet:
                    return BulletRepoShared.smallBullet;
                case BulletPrefabs.BigBullet:
                    return BulletRepoShared.bigBullet;
                case BulletPrefabs.HeartBullet:
                    return BulletRepoShared.heartBullet;
                default:
                    return BulletRepoShared.smallBullet;
            }
        }
    }

    public enum BulletPrefabs
    {
        SmallBullet,
        BigBullet,
        HeartBullet
    }
}