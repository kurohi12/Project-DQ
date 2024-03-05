using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : Singleton<EnemyManager>
{
    private LevelManager levelManager;
    private FSMEnemy enemyComponent;

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
        WaitForSeconds wait = new WaitForSeconds(LevelManager.Instance.patterns[patternNumber].CreateDelay);
        GameObject go = LevelManager.Instance.patterns[patternNumber].SpawnObject;
        for (int n = 0; n < LevelManager.Instance.patterns[patternNumber].SpawnCount; n++)
        {
            GameObject enemy = PoolManager.Instance.Spawn(go.gameObject.name);
            enemyComponent = enemy.GetComponent<FSMEnemy>();
            //enemyComponent.Setting = true;
            enemyComponent.wayPoints = LevelManager.Instance.patterns[patternNumber].wayPoints.ToList();
            enemy.transform.position = LevelManager.Instance.patterns[patternNumber].SpawnPosition.position;
            enemyComponent.Item = LevelManager.Instance.patterns[patternNumber].Item[n];
            yield return wait;
        }
    }
}
