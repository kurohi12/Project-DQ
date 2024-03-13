using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_LevelManager : MonoBehaviour
{
    private float nowTime = 0;

    [SerializeField]
    private int nowLevel = 0;

    [SerializeField]
    private Transform spawnPoint;

    List<Dictionary<string, object>> data;

    private void Awake()
    {
        data = CSVReader.Read("Test");
    }

    private void Update()
    {
        if (GameManager.Instance.gameStart == false)
        {
            return;
        }
       // nowTime = GameManager.Instance.GameTime;
        nowTime += Time.deltaTime;
        //특정 시간에 패턴 루틴 시작
        if (nowLevel >= 1) return;
        if (float.Parse(data[nowLevel]["SpawnTime"].ToString()) <= nowTime)
        {
            StartCoroutine(Sumon(nowLevel));
            nowLevel++;
        }
    }

    private IEnumerator Sumon(int level)
    {
        FSMEnemy enemyComponent;
        WaitForSeconds wait = new WaitForSeconds(float.Parse(data[level]["Delay"].ToString()));
        for (int n = 0; n < int.Parse(data[level]["SpawnCnt"].ToString()); n++)
        {
            GameObject enemy = PoolManager.Instance.Spawn(data[level]["Enemy"].ToString());
            enemyComponent = enemy.GetComponent<FSMEnemy>();
            WayPoints wayPoint = new WayPoints();
            enemyComponent.Speed = float.Parse(data[0]["Speed"].ToString());
            enemyComponent.Hp = float.Parse(data[0]["Hp"].ToString());
            for (int i = 0; i < int.Parse(data[0]["wayPointCnt"].ToString()); i++)
            {
                string x = (E_Way.x1 + (2*i)).ToString();
                string y = (E_Way.y1 + (2*i)).ToString();
                wayPoint.way[i] = new Vector3(float.Parse(data[0][x].ToString()), float.Parse(data[0][y].ToString()), 0);
            }
            enemyComponent.wayPoints.Add(wayPoint);
            enemy.transform.position = spawnPoint.position;
           // enemyComponent.Item = LevelManager.Instance.patterns[level].Item[n];
            yield return wait;
        }
    }
}
