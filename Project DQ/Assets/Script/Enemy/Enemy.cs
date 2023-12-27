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
    [SerializeField]
    private bool setting=false;

    private float t;

    private Vector3 bPosition;

    public bool Setting
    {
        get { return setting; }
        set { setting = value; }
    }

    public Vector3[] WayPoint { get { return (Vector3[])wayPoint.Clone(); } set { wayPoint = (Vector3[])value.Clone(); } }

    public GameObject Item { get { return (GameObject)item; } set { item = (GameObject)value; } }

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

    private void OnEnable()
    {
        t = 0;
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        t += Time.deltaTime * speed;

        bPosition = Mathf.Pow(1 - t, 3) * wayPoint[0]
                   + 3 * t * Mathf.Pow(1 - t, 2) * wayPoint[1]
                   + 3 * t * (1 - t) * wayPoint[2]
                   + Mathf.Pow(t, 3) * wayPoint[3];

        transform.position = bPosition;
        //t = 0;
        if (HP <= 0)
        {
            gameObject.SetActive(false);

            GameManager.Instance.Point += 10;
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

    private void OnDisable()
    {
        item.SetActive(true);
        setting = false;
    }
}
