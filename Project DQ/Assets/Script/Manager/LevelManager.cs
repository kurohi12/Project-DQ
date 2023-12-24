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
        public Transform spawnPosition;
        public float spawnTime;
        public float createTime;
        public int spawnCount;
        public GameObject[] itemCode;
        public Vector3[] wayPoint;
        public bool stop;
        public int spawnNum;
    }

    [SerializeField]
    public SumonPattern[] pattern;

    private float nowTime = 0;

    private int nowLevel = 0;
    private int returnlevel = 0;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        nowTime = GameManager.Instance.GameTime;

        if (pattern[nowLevel].spawnTime <= nowTime)
        {
            returnlevel = nowLevel;
            pattern[nowLevel].stop = false;
            StartCoroutine(EnemyManager.Instance.SelectSumon(nowLevel));
            nowLevel++;
        }

        if (pattern[returnlevel].stop)
        {
            StopCoroutine(EnemyManager.Instance.SelectSumon(returnlevel));
        }
    }

}
