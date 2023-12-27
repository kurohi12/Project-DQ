using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : Singleton<EnemyManager>
{
    [Serializable]
    public struct EnemyPool
    {
        public int poolSize; //오브젝트 풀 크기
        public GameObject[] enemyObj; //저장된 오브젝트
        public GameObject prefab; //프리팹
    }

    [SerializeField]
    private EnemyPool[] pool;

    public int PoolSize
    {
        get { return pool.Length; }
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (pool == null)
            return;

        //오브젝트 풀링
        for (int i = 0; i < pool.Length; i++)
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
    private void Update()
    {
        
    }

    //에너미 생성 루틴
    public IEnumerator Sumon(int patternNumber)
    {
        WaitForSeconds wait = new WaitForSeconds(LevelManager.Instance.pattern[patternNumber].createDelay);
        int number = LevelManager.Instance.pattern[patternNumber].poolNum;
        for (int n = 0; n < LevelManager.Instance.pattern[patternNumber].spawnCount; n++)
        {
            
            for (int i = 0; i < pool[number].poolSize; i++)
            {
                GameObject enemy = pool[number].enemyObj[i];

                if (!enemy.GetComponent<Enemy>().Setting)
                {
                    enemy.GetComponent<Enemy>().Setting = true;
                    enemy.GetComponent<Enemy>().WayPoint = LevelManager.Instance.pattern[patternNumber].wayPoint;
                    enemy.transform.position = LevelManager.Instance.pattern[patternNumber].spawnPosition.position;
                    enemy.GetComponent<Enemy>().Item = LevelManager.Instance.pattern[patternNumber].item[n];
                    enemy.SetActive(true);
                    break;
                }
            }
            yield return wait;
        }
        LevelManager.Instance.pattern[patternNumber].stop = true;
    }
}
