using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [Serializable]
    public struct SumonPattern
    {
        public Transform spawnPosition; //소환좌표
        public float spawnTime; //소환 타이밍
        public float createDelay; // 생성 딜레이
        public int spawnCount; //소환 개수
        public GameObject[] item; //아이템 (소환 개수에 맞춰야함)
        public Vector3[] wayPoint; //베지어 곡선 좌표 (4개)
        public bool stop; //코루틴 멈춤
        public int poolNum; //풀 넘버
    }

    [SerializeField]
    public SumonPattern[] pattern;

    private float nowTime = 0;

    private int nowLevel = 0;
    private int returnlevel = 0;

    private EnemyManager enemyManager;
    private GameManager gameManager;

    // Start is called before the first frame update
    private void Start()
    {
        enemyManager = EnemyManager.Instance;
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    private void Update()
    {

        //패턴 루틴 종료
        if (pattern[returnlevel].stop)
        {
            StopCoroutine(enemyManager.Sumon(returnlevel));
        }

        nowTime = gameManager.GameTime;
        
        if (pattern.Length<=nowLevel)
        {
            return;
        }

        //특정 시간에 패턴 루틴 시작
        if (pattern[nowLevel].spawnTime <= nowTime)
        {
            returnlevel = nowLevel;
            pattern[nowLevel].stop = false;
            StartCoroutine(enemyManager.Sumon(nowLevel));
            nowLevel++;
        }

        
    }

}
