using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    public float maxShotDelay;// 실제 딜레이
    public float curShotDelay;// 한번 발사후 다음 발사까지의 딜레이
    public float speed;
    public int power;
    public int life;
    
    Vector3 cPos = Vector3.zero;

    public GameObject[] PlayerBullet = new GameObject[4];
    public GameObject ScoreProtector; // 프로텍터를 저장할 변수
    
    // [스코어프로텍트함수]
    public bool shiftCheck = false;
    private bool prevShiftState = false;

    private void Update()
    {
        Move();
        Fire();
        Reload();
        bool currentShiftState = Input.GetButton("leftShift");
        if (currentShiftState != prevShiftState) // Shift 상태가 변경됐는지 확인
        {
            if (currentShiftState) // Shift 눌렸을 때
            {
                shiftCheck = true;
            }
            else // Shift 떼어졌을 때
            {
                shiftCheck = false;
            }

            Protector(); // 상태에 따라 처리
        }
        prevShiftState = currentShiftState; // 현재 상태를 이전 상태로 갱신

        CameraIn();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");

        float v = Input.GetAxisRaw("Vertical");

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;
    }

    private void Protector()
    {
        if (shiftCheck)
        {
            ScoreProtector.SetActive(true);
            speed = 1;
        }
        else
        {
            ScoreProtector.SetActive(false);
            speed = 3;
        }
    }

    private void Fire()
    {
        if (curShotDelay < maxShotDelay)// 현재 샷 딜레이가 본인이 설정한 맥스 샷 딜레이를 넘지 않았다면 아직 장전이 안된것이기에 반환시킴
            return;
        if (Input.GetButton("Fire1"))
        {
            // Instantiate = 매개변수 오브젝트를 생성하는 함수
            // (프리펩(original),생성 될 위치(position),오브젝트의 방향(rotation))
            BulletManager.Instance.BulletOn(power);
            // GameObject bullet = Instantiate(PlayerBullet[i], transform.position, transform.rotation);
            curShotDelay = 0;
        }
    }

    public int Power
    {
        get { return power; }
        set { power = value; }
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;// 딜레이 변수에 Time.deltaTime을 계속 더해 시간 계산
    }

    // 카메라 밖으로 못 나가게 만듦
    private void CameraIn()
    {
        cPos = Camera.main.WorldToViewportPoint(transform.position);

        if (cPos.x < 0f) cPos.x = 0f;

        if (cPos.x > 1f) cPos.x = 1f;

        if (cPos.y < 0f) cPos.y = 0f;

        if (cPos.y > 1f) cPos.y = 1f;

        transform.position = Camera.main.ViewportToWorldPoint(cPos);
    }
}
