using UnityEngine;

namespace Weapons
{
    public class BulletRepo : MonoBehaviour
    {
        public static BulletRepo BulletRepoShared { get; private set; }

        [SerializeField] private GameObject smallBullet;
        [SerializeField] private GameObject bigBullet;
        [SerializeField] private GameObject heartBullet;
        [SerializeField] private GameObject vaccineBag;

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
                case BulletPrefabs.VaccineBag:
                    return BulletRepoShared.vaccineBag;
                default:
                    return BulletRepoShared.smallBullet;
            }
        }
    }

    public enum BulletPrefabs
    {
        SmallBullet,
        BigBullet,
        HeartBullet,
        VaccineBag
    }
}