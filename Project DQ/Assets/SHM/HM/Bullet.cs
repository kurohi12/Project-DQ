using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;
    public int damage = 10;

    private void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    // 충돌 감지
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other != null)
            {
                other.gameObject.GetComponent<FSMEnemy>().Damaged(damage);
                GameManager.Instance.Point += 10;
                GameManager.Instance.Score();
            }
            // 총알 삭제
            PoolManager.Instance.Despawn(gameObject);
        }
    }
}
