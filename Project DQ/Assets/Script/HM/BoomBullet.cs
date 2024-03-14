using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBullet : MonoBehaviour
{
    [SerializeField]
    public float originalSpeed; // �ʱ갪 ���ǵ�

    [SerializeField]
    private GameObject player;

    public int damage = 10;
    public float nomalExplosionSize; // �⺻ ���� ���� ������
    public float growthDuration; // ũ�Ⱑ Ŀ���� �ð�

    private float speed;
    private float explosionSize; // ���� ���� ���� ������
    private bool isExploding = false; // ���� ���� üũ
    private float elapsedTime = 0f;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    private void OnEnable()
    {
        explosionSize = player.GetComponent<player>().power * nomalExplosionSize;
        speed = originalSpeed;             
    }

    private void Update()
    {
        if (isExploding)
        {
            elapsedTime += Time.deltaTime;
            float scaleFactor = Mathf.Lerp(0.1f, explosionSize, elapsedTime / growthDuration);
            transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            if (elapsedTime >= growthDuration)
            {
                PoolManager.Instance.Despawn(gameObject);
                isExploding = false;

                // ���� �Ѿ��� ���� �ʱ�ȭ
                elapsedTime = 0f;
                transform.localScale = new Vector3(0.1f,0.1f,0.1f);
                speed = originalSpeed; // �ʱ� �ӵ��� �����ϴ� ����
            }
        }
        else
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
    }

    // �浹 üũ
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if(isExploding != true)
            {
                isExploding = true;
                elapsedTime = 0f;
                speed = 0f;
            }
            if (other != null)
            {
                other.gameObject.GetComponent<FSMEnemy>().Damaged(damage);
            }
            //StartExplosion();
        }
    }

    // ���� �Լ�
    void StartExplosion()
    {
        isExploding = true;
        elapsedTime = 0f;
        speed = 0f;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionSize);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                FSMEnemy enemy = hitCollider.GetComponent<FSMEnemy>();
                if (enemy != null)
                {
                    enemy.gameObject.GetComponent<FSMEnemy>().Damaged(damage);
                }
            }
        }
    }
}
