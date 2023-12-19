using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    void Update()
    {
        Fire();
    }

    void Fire()
    {
        Rigidbody rigid = gameObject.GetComponent<Rigidbody>();
        rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);//Rigidbody를 가져와 AddForce로 총알 발사
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "BorderBullet")//총알 제거 경계를위한 새로운 태그로 조건 걸기
        {
            Destroy(gameObject);
        }
    }
}
