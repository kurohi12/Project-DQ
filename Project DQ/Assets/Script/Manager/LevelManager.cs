using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System.Linq;

[Serializable]
public class WayPoints
{
    public Vector3[] way = new Vector3[4];
}

[Serializable]
public struct SumonPattern
{
    public Transform SpawnPosition; //소환좌표
    public float SpawnTime; //소환 타이밍
    public float CreateDelay; // 생성 딜레이
    public int SpawnCount; //소환 개수
    public GameObject[] Item; //아이템 (소환 개수에 맞춰야함)
    public List<WayPoints> wayPoints; //베지어 곡선 좌표 (4개)
    public GameObject SpawnObject; //소환할 오브젝트
}

public class LevelManager : Singleton<LevelManager>
{

    [SerializeField]
    public List<SumonPattern> patterns = new List<SumonPattern>();

    private float nowTime = 0;

    [SerializeField]
    private int nowLevel = 0;

    public void GameStart()
    {
        nowLevel = 0;
        nowTime = 0;
    }

    protected override void Init()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(GameManager.Instance.gameStart == false)
        {
            return;
        }
        nowTime = GameManager.Instance.GameTime;
        if(nowLevel > patterns.Count)
        {
            return;
        }
        //특정 시간에 패턴 루틴 시작
        if (patterns[nowLevel].SpawnTime <= nowTime)
        {
            StartCoroutine(EnemyManager.Instance.Sumon(nowLevel));
            nowLevel++;
        }
    }


}
