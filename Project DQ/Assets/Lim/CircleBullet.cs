using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBullet : MonoBehaviour
{
    [SerializeField]
    private int numberOfBullets = 20; //  ź�� ��
    [SerializeField]
    private float spreadAngle = 360f; // ź�� �л� ���� (��: 45��)
    [SerializeField]
    private float midAngle = 2f; // �߽� źȯ ����

    [SerializeField]
    private GameObject bulletPrefab; // ź�� ������
    

    private LBullet Bullet;

    private void Start()
    {
        
    }

    public void Shoot()
    {
        //ź�� ����
        float angleStep = spreadAngle / (numberOfBullets - 1);

        //�߽� ����
        float startAngle = -spreadAngle / midAngle;

        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector3 direction = Quaternion.Euler(0, 0, angle) * -transform.right;

            GameObject bullet = PoolManager.Instance.Spawn(bulletPrefab.gameObject.name);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;
            Bullet = bullet.GetComponent<LBullet>();
            bullet.transform.position += direction;

            Bullet.Direction = direction;
        }
    }
}
