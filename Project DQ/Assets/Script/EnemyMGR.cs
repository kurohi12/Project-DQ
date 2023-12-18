using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.Linq;

public class EnemyMGR : MonoBehaviour
{
    [Serializable]
    public struct EnemyPool
    {
        public int poolSize; //오브젝트 풀 크기
        public GameObject[] enemyObj; //저장된 오브젝트
        public Transform spawnPoint; //소환 좌표
        public GameObject prefab; //프리팹
        public float createTime;
        public int spawnCnt;
    }

    [SerializeField]
    private EnemyPool[] pool;

    // Start is called before the first frame update
    void Start()
    {

        //오브젝트 풀링
        for(int i=0;i<pool.Length; i++)
        {
            //오브젝트 풀을 에너미들을 담을 수 있는 크기로 설정
            pool[i].enemyObj = new GameObject[pool[i].poolSize];
            //오브젝트 풀에 넣을 에너미 개수만큼 반복
            for (int j = 0; j < pool[i].poolSize; j++)
            {
                //에너미 생성
                GameObject enemy = Instantiate(pool[i].prefab);
                //에너미를 오브젝트 풀에 입력
                pool[i].enemyObj[j] = enemy;
                //비활성화
                enemy.SetActive(false);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Sumon());
        }
    }

    IEnumerator Sumon()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            //소환 타임이 되면 적 소환
            for (int n = 0; n < pool[i].spawnCnt; n++)
            {
                //에너미풀 안에 있는 에너미에서
                for (int j = 0; j < pool[i].poolSize; j++)
                {
                    GameObject enemy = pool[i].enemyObj[j];
                    //만약 에너미가 비활성화라면
                    if (enemy.activeSelf == false)
                    {
                        //에너미 위치
                        enemy.transform.position = pool[i].spawnPoint.position;
                        //에너미 활성화
                        enemy.SetActive(true);
                        //검색 중단
                        break;
                    }
                }
                yield return new WaitForSeconds(pool[i].createTime);
            }
        }
    }
}
