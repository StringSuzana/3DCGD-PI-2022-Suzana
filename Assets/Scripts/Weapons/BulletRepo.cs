using MyGame;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Weapons
{
    public class BulletRepo : MonoBehaviour
    {
        public static BulletRepo BulletRepoShared { get; private set; }

        [SerializeField] private GameObject hand;
        [SerializeField] private GameObject heart;
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
            if (SceneManager.GetActiveScene().name == LevelNames.FirstLevel)
            {
                switch (prefab)
                {
                    case BulletPrefabs.Hand:
                        return BulletRepoShared.hand;
                    case BulletPrefabs.Heart:
                        return BulletRepoShared.heart;
                    default:
                        return BulletRepoShared.hand;
                }
            }
            else
            {
                switch (prefab)
                {
                    case BulletPrefabs.Hand:
                        return BulletRepoShared.hand;
                    case BulletPrefabs.Heart:
                        return BulletRepoShared.heart;
                    case BulletPrefabs.VaccineBag:
                        return BulletRepoShared.vaccineBag;
                    default:
                        return BulletRepoShared.hand;
                }
            }

           
        }
    }

    public enum BulletPrefabs
    {
        Hand,
        Heart,
        VaccineBag
    }
}