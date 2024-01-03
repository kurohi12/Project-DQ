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

    private LevelManager levelManager;

    private GameObject enemy;
    private Enemy enemyComponent;

    public int PoolSize
    {
        get { return pool.Length; }
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (pool == null)
            return;

        levelManager = LevelManager.Instance;

        //오브젝트 풀링
        for (int i = 0; i < pool.Length; i++)
        {
            //오브젝트 풀을 에너미들을 담을 수 있는 크기로 설정
            pool[i].enemyObj = new GameObject[pool[i].poolSize];
            //오브젝트 풀에 넣을 에너미 개수만큼 반복
            for (int j = 0; j < pool[i].poolSize; j++)
            {
                //에너미 생성
                enemy = Instantiate(pool[i].prefab);
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
        WaitForSeconds wait = new WaitForSeconds(levelManager.pattern[patternNumber].createDelay);
        int number = levelManager.pattern[patternNumber].poolNum;
        for (int n = 0; n < levelManager.pattern[patternNumber].spawnCount; n++)
        {
            
            for (int i = 0; i < pool[number].poolSize; i++)
            {
                enemy = pool[number].enemyObj[i];
                enemyComponent = enemy.GetComponent<Enemy>();
                if (!enemyComponent.Setting)
                {
                    enemyComponent.Setting = true;
                    enemyComponent.WayPoint = levelManager.pattern[patternNumber].wayPoint;
                    enemy.transform.position = levelManager.pattern[patternNumber].spawnPosition.position;
                    enemyComponent.Item = levelManager.pattern[patternNumber].item[n];
                    enemy.SetActive(true);
                    break;
                }
            }
            yield return wait;
        }
        levelManager.pattern[patternNumber].stop = true;
    }
}
