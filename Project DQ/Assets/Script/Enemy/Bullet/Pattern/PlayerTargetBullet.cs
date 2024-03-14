using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetBullet : MonoBehaviour
{
    [SerializeField]
    private GameObject targetType;
    [SerializeField]
    private GameObject bulletPrefab; // ÅºÀÇ ÇÁ¸®ÆÕ

    private GameObject bullet;


    private EnemyBullet enemyBullet;

    private void Awake()
    {
        targetType = GameObject.Find("Player");
    }

    private void OnEnable()
    {
        
    }

    public void Shoot()
    {
        Vector3 dir = targetType.transform.position - transform.position;
        dir.Normalize();
        bullet = Instantiate(bulletPrefab);
        bullet.transform.position = transform.position;
        enemyBullet = bullet.GetComponent<EnemyBullet>();

        enemyBullet.Direction = dir;
    }
}
