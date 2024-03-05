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
    public Transform SpawnPosition; //��ȯ��ǥ
    public float SpawnTime; //��ȯ Ÿ�̹�
    public float CreateDelay; // ���� ������
    public int SpawnCount; //��ȯ ����
    public GameObject[] Item; //������ (��ȯ ������ �������)
    public List<WayPoints> wayPoints; //������ � ��ǥ (4��)
    public GameObject SpawnObject; //��ȯ�� ������Ʈ
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
        //Ư�� �ð��� ���� ��ƾ ����
        if (patterns[nowLevel].SpawnTime <= nowTime)
        {
            StartCoroutine(EnemyManager.Instance.Sumon(nowLevel));
            nowLevel++;
        }
    }


}
