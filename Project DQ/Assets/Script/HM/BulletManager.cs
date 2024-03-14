using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //  [Serializable]

[Serializable]
public class PlayerType
{
    public GameObject prefab; // ÇÁ¸®Æé
    public Transform spawnPoint; //ÇÃ·¹ÀÌ¾î ÁÂÇ¥
}

public class BulletManager : Singleton<BulletManager>
{
    [Serializable]
    public struct Bullet
    {
       public List<PlayerType> playerTypes;
    }

    [SerializeField]
    private Bullet[] pool;

    [SerializeField]
    private Transform[] changePoint;

    [SerializeField]
    private GameObject player;
    private player playerComponent;

    private void Start()
    {
        playerComponent = player.GetComponent<player>();
    }

    public void ChangePosition(float power)
    {
        switch((int)power)
        {
            case 1:
                
                break;
            case 2:
                pool[0].playerTypes[2].spawnPoint = changePoint[2];
                break;
            case 3:
                pool[0].playerTypes[2].spawnPoint = changePoint[3];
                break;
            case 4:
                pool[0].playerTypes[4].spawnPoint = changePoint[2];
                break;
            case 5:
                pool[0].playerTypes[2].spawnPoint = changePoint[3];
                pool[0].playerTypes[4].spawnPoint = changePoint[6];
                break;


        }
    }

    public void BulletOn(float Power,int playerType)
    {
        if(Power > 5)
        {
            Power = 5;
        }
        for (int i = 0; i < (int)Power+1; i++)
        {
            GameObject bullet = PoolManager.Instance.Spawn(pool[playerType].playerTypes[i].prefab.gameObject.name);
            bullet.transform.position = pool[playerType].playerTypes[i].spawnPoint.position;
        }
    }

}

