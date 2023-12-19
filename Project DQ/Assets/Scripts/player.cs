using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchLeft;
    public bool isTouchRight;

    public float maxShotDelay;//실제 딜레이
    public float curShotDelay;//한번 발사후 다음 발사까지의 딜레이
    public float speed;
    public int power;

    public GameObject[] PlayerBullet = new GameObject[4];

    void Update()
    {
        Move();
        Fire();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if ((isTouchRight && h == -1) || (isTouchLeft && h == 1)) //오,왼 벽 충돌체크
            h = 0;

        float v = Input.GetAxisRaw("Vertical");
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1)) //위,아래 벽 충돌체크
            v = 0;

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1"))
            return;

        if (curShotDelay < maxShotDelay)//현재 샷 딜레이가 본인이 설정한 맥스 샷 딜레이를 넘지 않았다면 아직 장전이 안된것이기에 반환시킴
            return;

        //Instantiate = 매개변수 오브젝트를 생성하는 함수
        //(프리펩(original),생성 될 위치(position),오브젝트의 방향(rotation))
        for(int i = 0;i < Power;i++)
        {
            GameObject bullet = Instantiate(PlayerBullet[i], transform.position, transform.rotation);
        }
    }

    public int Power
    {
        get { return power; }
        set { power = value; }
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;//딜레이 변수에 Time.deltaTime을 계속 더해 시간 계산
    }

    void OnTriggerEnter(Collider collision)//벽에 닿았을시 체크
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
    }

    void OnTriggerExit(Collider collision)//벽에 닿지 않았을때의 체크
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
}
