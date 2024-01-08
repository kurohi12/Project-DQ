using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : Singleton<EnemyManager>
{
    /*[Serializable]
    public struct EnemyPool
    {
        public int poolSize; //오브젝트 풀 크기
        public GameObject[] enemyObj; //저장된 오브젝트
        public GameObject prefab; //프리팹
    }*/

   /* [SerializeField]
    private EnemyPool[] pool;*/

    private LevelManager levelManager;
    private Enemy enemyComponent;

    // Start is called before the first frame update
    private void Start()
    {
        
        levelManager = LevelManager.Instance;
       
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    //에너미 생성 루틴
    public IEnumerator Sumon(int patternNumber)
    {
        WaitForSeconds wait = new WaitForSeconds(LevelManager.Instance.pattern[patternNumber].createDelay);
        GameObject go = levelManager.pattern[patternNumber].spawnObject;
        for (int n = 0; n < levelManager.pattern[patternNumber].spawnCount; n++)
        {
            GameObject enemy = PoolManager.Instance.Spawn(go.gameObject.name);
            enemyComponent = enemy.GetComponent<Enemy>();
            enemyComponent.Setting = true;
            enemyComponent.WayPoint = levelManager.pattern[patternNumber].wayPoint;
            enemy.transform.position = levelManager.pattern[patternNumber].spawnPosition.position;
            enemyComponent.Item = levelManager.pattern[patternNumber].item[n];
            yield return wait;
        }
        LevelManager.Instance.pattern[patternNumber].stop = true;
    }
}
