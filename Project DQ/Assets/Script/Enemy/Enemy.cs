using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 3; //움직이는 속도
    [SerializeField]
    private int hp = 10; //체력
    [SerializeField]
    private Vector3[] wayPoint = new Vector3[4]; //베지어 곡선 좌표
    [SerializeField]
    private GameObject item = null; //드롭 아이템

    private float t;

    Vector3 dir = Vector3.zero;

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }

    public Vector3 Direction
    {
        get { return dir; }
        set { dir = value; }
    }

    private void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        StartCoroutine(BezierLining());
        if (HP <= 0)
        {
            gameObject.SetActive(false);

            //아래에 플레이어 점수 증가 코드 필요
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //플레이어 탄막
        if(collision.gameObject.CompareTag("PlayerBullet"))
        {
            hp--;
        }
    }

    private void Dead()
    {
        item.SetActive(true);
    }

    private IEnumerator BezierLining()
    {
        WaitForEndOfFrame frame = new WaitForEndOfFrame();
        Vector3 bPosition;
        
        t += Time.deltaTime * speed;

        while (t < 1)
        {
            bPosition = Mathf.Pow(1 - t, 3) * wayPoint[0]
                    + 3 * t * Mathf.Pow(1 - t, 2) * wayPoint[1]
                    + 3 * t * (1 - t) * wayPoint[2]
                    + Mathf.Pow(t, 3) * wayPoint[3];

            transform.position = bPosition;

            yield return frame;
        }
        t = 0;
    }

    private void OnDisable()
    {
        t = 0;
    }
}
