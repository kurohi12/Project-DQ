using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBullet : MonoBehaviour
{
    [SerializeField]
    public float originalSpeed; // 초깃값 스피드

    [SerializeField]
    private GameObject player;

    public int damage = 10;
    public float nomalExplosionSize; // 기본 폭발 범위 사이즈
    public float growthDuration; // 크기가 커지는 시간

    private float speed;
    private float explosionSize; // 변경 폭발 범위 사이즈
    private bool isExploding = false; // 폭발 여부 체크
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

                // 다음 총알을 위한 초기화
                elapsedTime = 0f;
                transform.localScale = new Vector3(0.1f,0.1f,0.1f);
                speed = originalSpeed; // 초기 속도를 저장하는 변수
            }
        }
        else
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
    }

    // 충돌 체크
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

    // 폭발 함수
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
