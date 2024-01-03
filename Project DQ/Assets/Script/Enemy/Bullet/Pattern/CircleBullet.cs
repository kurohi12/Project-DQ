using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBullet : MonoBehaviour
{
    [SerializeField]
    private int numberOfBullets = 20; //  ÅºÀÇ ¼ö
    [SerializeField]
    private float spreadAngle = 360f; // ÅºÀÇ ºÐ»ê °¢µµ (¿¹: 45µµ)
    [SerializeField]
    private float midAngle = 2f; // Áß½É ÅºÈ¯ °¢µµ

    [SerializeField]
    private GameObject bulletPrefab; // ÅºÀÇ ÇÁ¸®ÆÕ
    private GameObject bullet;

    private EnemyBullet enemyBullet;

    private void Start()
    {
        
    }

    public void Shoot()
    {
        //Åº¸· °¢µµ
        float angleStep = spreadAngle / (numberOfBullets - 1);

        //Áß½É °¢µµ
        float startAngle = -spreadAngle / midAngle;

        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector3 direction = Quaternion.Euler(0, 0, angle) * -transform.right;

            bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            enemyBullet = bullet.GetComponent<EnemyBullet>();
            bullet.transform.position += direction;

            enemyBullet.Direction = direction;
        }
    }
}
