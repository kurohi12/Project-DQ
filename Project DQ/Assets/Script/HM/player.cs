using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ����Ƽ Scene

public enum States
{
    PlayerOne,
    PlayerTwo,
    PlayerThree
}

public class player : MonoBehaviour
{
    public float maxShotDelay;// ���� ������
    public float curShotDelay;// �ѹ� �߻��� ���� �߻������ ������
    public float speed;
    public float power;
    public int life;
    public bool isHit;
    public bool save = false;
    
    Vector3 cPos = Vector3.zero;

    //public GameObject[] PlayerBullet = new GameObject[4];
    public GameObject ScoreProtector; // �������͸� ������ ����
    
    // [���ھ�������Ʈ�Լ�]
    public bool shiftCheck = false;
    private bool prevShiftState = false;

    [SerializeField]
    private States playerType;

    public GameManager manager;
    public Text powerText;


    private void Awake()
    {
        // ����� ���� ĳ���� �⺻�� ����
        playerType = PlayerManager.Instance.playerType;
        power = 1;
    }

    private void Update()
    {
        Move();     
        bool currentShiftState = Input.GetButton("leftShift");
        if (Input.GetKeyDown(KeyCode.Space)) // �ӽ�
        {
            LoadManager.Instance.sceneNum = 5;
            SceneManager.LoadScene("LoadingScene3");
        }
        if (Input.GetKey(KeyCode.Z))
        {
            if (curShotDelay >= maxShotDelay)// ���� �� �����̰� ������ ������ �ƽ� �� �����̸� ���� �ʾҴٸ� ���� ������ �ȵȰ��̱⿡ ��ȯ��Ŵ
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
        if (currentShiftState != prevShiftState) // Shift ���°� ����ƴ��� Ȯ��
        {
            if (currentShiftState) // Shift ������ ��
            {
                shiftCheck = true;
            }
            else // Shift �������� ��
            {
                shiftCheck = false;
            }

        }
        Protector(); // ���¿� ���� ó��
        prevShiftState = currentShiftState; // ���� ���¸� ���� ���·� ����
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
        curShotDelay += Time.deltaTime;// ������ ������ Time.deltaTime�� ��� ���� �ð� ���
    }

    // ī�޶� ������ �� ������ ����
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
