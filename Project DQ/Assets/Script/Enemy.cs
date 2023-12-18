using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    protected float speed = 3;
    [SerializeField]
    protected int hp = 10;
    [SerializeField]
    protected Vector3[] wayPoint = new Vector3[4];

    private Vector3 bPosition;
    protected float t;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (HP <= 0)
        {
            gameObject.SetActive(false);

            //아래에 플레이어 점수 증가 코드 필요
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        //플레이어 탄막
        if(collision.gameObject.tag == "Bullet_P")
        {
            GameObject bullet = (GameObject)collision.gameObject;
            
            //아래에는 플레이어 공격력에 따라 체력 감소 코드 필요
        }
    }

    protected IEnumerator BezierLining()
    {
        t += Time.deltaTime * speed;

        while (t < 1)
        {
            bPosition = Mathf.Pow(1 - t, 3) * wayPoint[0]
                    + 3 * t * Mathf.Pow(1 - t, 2) * wayPoint[1]
                    + 3 * t * (1 - t) * wayPoint[2]
                    + Mathf.Pow(t, 3) * wayPoint[3];

            transform.position = bPosition;

            yield return new WaitForEndOfFrame();
        }
        t = 0;
    }
}
