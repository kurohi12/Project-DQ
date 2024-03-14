using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NWayBullet : MonoBehaviour
{
    [SerializeField]
    private int numberOfBullets = 20; //  ź�� ��
    [SerializeField]
    private float spreadAngle = 45f; // ź�� �л� ���� (��: 45��)

    [SerializeField]
    private GameObject bulletPrefab; // ź�� ������
    private GameObject bullet;

    private EnemyBullet enemyBullet;

    private void Start()
    {
        
    }

    public void Shoot()
    {
        //ź�� ����
        float angleStep = spreadAngle / (numberOfBullets - 1);

        //�߽� ����
        float startAngle = -spreadAngle / 2;

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
