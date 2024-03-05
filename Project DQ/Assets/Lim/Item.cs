using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Item : MonoBehaviour
{

    private int speed = 5;

    private GameObject player;

    //아이템 코드 0=포인트 , 1=파워 레벨
    [SerializeField]
    private int code = 0;

    private float status = 0;

    public int Code
    {
        get { return code; }
        set { code = value; }
    }

    public float Status
    {
        get { return status; }
        set { status = value; }
    }

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position += -Vector3.right * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == player.name)
        {
            switch (code)
            {
                case 0:
                    GameManager.Instance.Point += 10;
                    GameManager.Instance.Score();
                    break;
                case 1:
                    player.GetComponent<player>().Power += 0.2f;
                    break;
            }
            PoolManager.Instance.Despawn(gameObject);
        }
        
    }
}
