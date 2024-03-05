using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboBullet : MonoBehaviour
{
    [SerializeField]
    private float nomalSpeed;
    public float damege;

    [SerializeField]
    private float comboDamege;
    private GameObject player;
    private float speed;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    private void OnEnable()
    {
        comboDamege = GameManager.Instance.KillCount * (player.GetComponent<player>().power / 2.0f);
        speed = player.GetComponent<player>().power * nomalSpeed;
    }

    private void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other != null)
            {
                other.gameObject.GetComponent<FSMEnemy>().Damaged(comboDamege + damege);
            }
            // ÃÑ¾Ë »èÁ¦
            PoolManager.Instance.Despawn(gameObject);
        }
    }
}
