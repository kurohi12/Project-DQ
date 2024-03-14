using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBullet : MonoBehaviour
{
    public float bulletSpeed;
    public int damage = 10;
    private Transform target;
    Vector3 dir = Vector3.zero;
    Vector3 direction = Vector3.zero;

    private void OnEnable()
    {
        FindClosestEnemy();
    }

    void Update()
    {
        float slopeAngle = 0;
        if (target != null)
        {
            if (target.gameObject.activeSelf)
            {
                // 방향 벡터 계산
                direction = target.position - transform.position;
                slopeAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, slopeAngle);
                dir = direction.normalized;
                // 총알 전진
                transform.position += dir * bulletSpeed * Time.deltaTime;
            }
            else transform.position += Vector3.right * bulletSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.right * bulletSpeed * Time.deltaTime;
        }
    }

    void FindClosestEnemy()
    {
        FSMEnemy[] enemies = FindObjectsOfType<FSMEnemy>();
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (FSMEnemy enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        target = closestEnemy;
    }

    // 충돌 감지
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other != null)
            {
                other.gameObject.GetComponent<FSMEnemy>().Damaged(damage);
            }
            // 총알 삭제
            PoolManager.Instance.Despawn(gameObject);
        }
    }

    private void OnDisable()
    {
        target = null;
    }
}
