using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRepo : MonoBehaviour
{
    public static BulletRepo bulletRepoShared { get; private set; }

    [SerializeField]
    private GameObject smallBullet;
    [SerializeField]
    private GameObject bigBullet;
    [SerializeField]
    private GameObject heartBullet;

    private void Awake()
    {
        if (bulletRepoShared == null)
        {
            bulletRepoShared = this;
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
            case BulletPrefabs.smallBullet:
                return bulletRepoShared.smallBullet;
            case BulletPrefabs.bigBullet:
                return bulletRepoShared.bigBullet;
            case BulletPrefabs.heartBullet:
                return bulletRepoShared.heartBullet;
            default:
                return bulletRepoShared.smallBullet;
        }
    }
}

public enum BulletPrefabs
{
    smallBullet, bigBullet, heartBullet
}