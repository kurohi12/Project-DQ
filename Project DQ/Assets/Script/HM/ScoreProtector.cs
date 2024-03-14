using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreProtector : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            GameManager.Instance.Point += 100;
            GameManager.Instance.Score();
            return;

        }
    }
}
