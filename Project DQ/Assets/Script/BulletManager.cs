using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //  [Serializable]

public class BulletManager : Singleton<BulletManager>
{
    [Serializable]
    public struct BulletPool
    {
        public GameObject prefab; // ÇÁ¸®Æé
        public Transform spawnPoint; //ÇÃ·¹ÀÌ¾î ÁÂÇ¥
    }

    [SerializeField]
    private BulletPool[] pool;

    [SerializeField]
    private Transform changePoint;

    [SerializeField]
    private Transform subchangePoint;

    [SerializeField]
    private GameObject player;
    private player playerComponent;

    private void Start()
    {
        playerComponent = player.GetComponent<player>();
    }

    private void Update()
    {
        if (playerComponent.power > 2)
        {
            pool[2].spawnPoint = changePoint;
        }
        if (playerComponent.power > 4)
        {
            pool[4].spawnPoint = subchangePoint;
        }
    }

    public void BulletOn(int Power)
    {
        for (int i = 0; i < Power+1; i++)
        {
            GameObject bullet = PoolManager.Instance.Spawn(pool[i].prefab.gameObject.name);
            bullet.transform.position = pool[i].spawnPoint.position;
        }
    }
}
