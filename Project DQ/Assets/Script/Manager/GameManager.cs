using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 유니티 UI
using UnityEngine.SceneManagement; // 유니티 Scene

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private float curTime;
    [SerializeField]
    private int playerPoint;
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    public int KillCount;

    public bool gameStart = false;
    public bool gameOver = false;

    public GameObject player;
    public Text scoreText;
    public Image[] lifeImage;
    public GameObject gameOverSet;
    

    public float GameTime
    {
        get { return curTime; }
        set { curTime = value; }
    }

    public int Point
    {
        get { return playerPoint; }
        set { playerPoint = value; }
    }

    public void GameStart()
    {
        curTime = 0;
        playerPoint = 0;
        KillCount = 0;
        gameStart = true;
        gameOver = false;
    }

    protected override void Awake()
    {
        
    }

    // Start is called before the first frame update
    private void Start()
    {
        curTime = 0;
        playerPoint = 0;
        KillCount = 0;
        gameStart = true;
        gameOver = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if(gameStart)
        {
            curTime += Time.deltaTime;
        }

        // UI Score Update
        
    }

    public void Score()
    {
        if (SceneManager.GetActiveScene().name == "UIScene")
        {
            scoreText.text = string.Format("{0:n0}", playerPoint);
        }
    }

    public void UpdateLifeIcon(int life) // life 아이콘 갱신
    {      
        lifeImage[life].enabled = false;       
    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2f);
    }

    private void RespawnPlayerExe()
    {
        player.transform.position = Vector3.left * 3.5f;
        player.SetActive(true);

        player playerLogic = player.GetComponent<player>();
        playerLogic.isHit = false;
        playerLogic.save = true;
        StartCoroutine(playerLogic.SaveTime());
    }

    public void GameOver()
    {
        gameStart = false;
        gameOver = true;
        ScoreManager.Instance.gameOver = true;
        StopAllCoroutines();
        FSMEnemy[] enemies = FindObjectsOfType<FSMEnemy>();

        foreach (FSMEnemy enemy in enemies)
        {
            PoolManager.Instance.Despawn(enemy.gameObject);
        }
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        player playerLogic = player.GetComponent<player>();
        playerLogic.ResetPlayer();
        playerPoint = 0;
        gameOverSet.SetActive(false);
        player.SetActive(true);
        player.transform.position = playerTransform.position;
        SceneManager.LoadScene("LoadingScene2");
    }
}
