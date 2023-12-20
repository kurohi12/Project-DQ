using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    public float maxShotDelay;//실제 딜레이
    public float curShotDelay;//한번 발사후 다음 발사까지의 딜레이
    public float speed;
    public int power;

    public GameObject[] PlayerBullet = new GameObject[4];

    void Update()
    {
        Move();
        Fire();
        Reload();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");

        float v = Input.GetAxisRaw("Vertical");

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;
    }

    void Fire()
    {
        if (curShotDelay < maxShotDelay)//현재 샷 딜레이가 본인이 설정한 맥스 샷 딜레이를 넘지 않았다면 아직 장전이 안된것이기에 반환시킴
            return;
        if (Input.GetButton("Fire1"))
        {
            //Instantiate = 매개변수 오브젝트를 생성하는 함수
            //(프리펩(original),생성 될 위치(position),오브젝트의 방향(rotation))
            for (int i = 0; i < Power; i++)
            {
                GameObject bullet = Instantiate(PlayerBullet[i], transform.position, transform.rotation);
            }
            curShotDelay = 0;
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
}
