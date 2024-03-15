using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 유니티 Scene

public enum States
{
    PlayerOne,
    PlayerTwo,
    PlayerThree
}

public class player : MonoBehaviour
{
    public float maxShotDelay;// 실제 딜레이
    public float curShotDelay;// 한번 발사후 다음 발사까지의 딜레이
    public float speed;
    public float power;
    public int life;
    public bool isHit;
    public bool save = false;
    
    Vector3 cPos = Vector3.zero;

    //public GameObject[] PlayerBullet = new GameObject[4];
    public GameObject ScoreProtector; // 프로텍터를 저장할 변수
    
    // [스코어프로텍트함수]
    public bool shiftCheck = false;
    private bool prevShiftState = false;

    [SerializeField]
    private States playerType;

    public GameManager manager;
    public Text powerText;


    private void Awake()
    {
        // 여기다 추후 캐릭터 기본값 설정
        playerType = PlayerManager.Instance.playerType;
        power = 1;
    }

    private void Update()
    {
        Move();     
        bool currentShiftState = Input.GetButton("leftShift");
        if (Input.GetKeyDown(KeyCode.Space)) // 임시
        {
            LoadManager.Instance.sceneNum = 5;
            SceneManager.LoadScene("LoadingScene3");
        }
        if (Input.GetKey(KeyCode.Z))
        {
            if (curShotDelay >= maxShotDelay)// 현재 샷 딜레이가 본인이 설정한 맥스 샷 딜레이를 넘지 않았다면 아직 장전이 안된것이기에 반환시킴
            {
                switch (playerType)
                {
                    case States.PlayerOne:
                        BulletManager.Instance.ChangePosition(power);
                        BulletManager.Instance.BulletOn(power,0);
                        break;
                    case States.PlayerTwo:
                        BulletManager.Instance.BulletOn(1,1);
                        break;
                    case States.PlayerThree:
                        BulletManager.Instance.BulletOn(1,2);
                        break;
                }
                curShotDelay = 0;
            }            
        }             
        Reload();   
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

        }
        Protector(); // 상태에 따라 처리
        prevShiftState = currentShiftState; // 현재 상태를 이전 상태로 갱신
        CameraIn();
        powerText.text = string.Format("{0:F1}", power);
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");

        float v = Input.GetAxisRaw("Vertical");

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            if (save)
                return;

            if (isHit)
                return;
            
            isHit = true;
            ScoreProtector.SetActive(false);
            life--;
            manager.UpdateLifeIcon(life);
            if(life == 0)
            {
                manager.GameOver();
            }
            else
            {
                manager.RespawnPlayer();
            }
            gameObject.SetActive(false);
        }
    }

    private void Protector()
    {
        if (shiftCheck)
        {
            ScoreProtector.SetActive(true);
            ScoreProtector.transform.position = this.transform.position;
            speed = 2;
        }
        else
        {
            ScoreProtector.SetActive(false);
            speed = 5;
        }
    }

    public float Power
    {
        get { return power; }
        set { power = value; }
    }

    public States PlayerType
    {
        get { return playerType; }
        set { playerType = value; }
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

    public void ResetPlayer()
    {
        life = 3;
        power = 1;
        isHit = false;
        manager.UpdateLifeIcon(life-1);
        for (int index = 0; index < life; index++)
        {
            manager.lifeImage[index].enabled = true;
        }
    }

    public IEnumerator SaveTime()
    {
        yield return new WaitForSeconds(5f);
        save = false;
        StopCoroutine(SaveTime());
    }
}
