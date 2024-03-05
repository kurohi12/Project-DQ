using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static BulletManager;
using static UnityEngine.GraphicsBuffer;

public class OneStageMidBoss : FSMEnemy
{
    [Serializable]
    public struct Level
    {
        public float HP;
        public float Speed;
        public List<WayPoints> movePattern;
        public float moveTime;
        public bool isRunning;
    }
    [Serializable]
    public struct CirclePattern
    {
        public int numberOfBullets; //  탄의 수
        public float spreadAngle; // 탄의 분산 각도 (예: 45도)
        public float midAngle; // 중심 탄환 각도
        public GameObject bulletPrefab;
    }
    [Serializable]
    public struct TargetPattern
    {
        public int numberOfBullets;
        public GameObject bulletPrefab;
    }
    [Serializable]
    public struct N_WayPattern
    {
        public int numberOfBullets; //  탄의 수
        public float spreadAngle; // 탄의 분산 각도 (예: 45도)
        public float midAngle; // 중심 탄환 각도
        public GameObject bulletPrefab;
    }
    [Serializable]
    public struct RazerPattern
    {
        public int numberOfBullets; //  탄의 수
        public GameObject bulletPrefab;
        public GameObject raser;
        public float size; //길이
    }

    [Serializable]
    public struct FirstPattern
    {
        public CirclePattern circlePattern;
        public TargetPattern targetPattern;
        public N_WayPattern n_WayPattern;
    }
    [Serializable]
    public struct SecondPattern
    {
        public N_WayPattern[] n_WayPatterns;
    }
    [Serializable]
    public struct ThirdPattern
    {
        public N_WayPattern n_WayPattern;
        public RazerPattern raserBullet;
    }

    [SerializeField]
    private GameObject target;

    [SerializeField] private FirstPattern firstPattern;
    [SerializeField] private SecondPattern secondPattern;
    [SerializeField] private ThirdPattern thirdPattern;



    [SerializeField]
    private Level[] level;

    [SerializeField]
    public int HPCount;
    //체력 브레이크/라운드 

    [SerializeField]
    private float spawnTime;
    private float stayTime = 0.0f;

    private bool save=false;

    private EnemyBullet enemyBullet;
    [SerializeField]
    private GameObject hpBar;

    protected override void Init() {
        target = GameObject.Find("Player");
        hpBar = GameObject.Find("BossHpUI");
        hpBar.transform.GetChild(0).gameObject.SetActive(true);
        hpBar.transform.GetChild(1).gameObject.SetActive(true);
        hpBar.transform.GetChild(2).gameObject.SetActive(true);
        HPCount = 2;
        stayTime = 0.0f;
        nextMove = 0;
        t = 0;
        Speed = 1f;
        wayPoints = level[HPCount].movePattern.ToList();
        Hp = level[HPCount].HP;
        hpBar.GetComponent<BossHpBar>().BossHpCount(HPCount);
        hpBar.GetComponent<BossHpBar>().MaxHp = Hp;
        hpBar.GetComponent<BossHpBar>().CurHp = Hp;
        state = State.MOVE;
    }
    protected override void StayOn() {
        stayTime += Time.deltaTime;

        if (stayTime > level[HPCount].moveTime)
        {
            wayPoints = level[HPCount].movePattern.ToList();
            stayTime = 0;
            t = 0;
            nextMove = UnityEngine.Random.Range(0, level[HPCount].movePattern.Count);
            wayPoints[nextMove].way[0] = gameObject.transform.position;
            state = State.MOVE;
        }
    }
    protected override void MoveOn() {
        if (HPCount == 0 && !level[0].isRunning)
        {
            level[2].isRunning = false;
            level[1].isRunning = false;
            StopCoroutine(PatternOne());
            StopCoroutine(PatternTwo());
            StartCoroutine(PatternThree());
        }
        StartCoroutine(MoveRoutine());
    }
    protected override void GoalOn() {
        StopCoroutine(MoveRoutine());
        if (level[0].isRunning)
        {
            StopCoroutine(PatternThree());
            level[0].isRunning = false;
            stayTime = 0;
            state = State.STAY;
            return;
        }

        if (level[HPCount].isRunning)
        {
            stayTime = 0;
            state = State.STAY;
        }
        else state = State.ATTACK;
    }
    protected override void AttackOn() {
        if(HPCount == 2 && !level[2].isRunning)
        {
            StartCoroutine(PatternOne());
        }
        if (HPCount == 1 && !level[1].isRunning)
        {
            level[2].isRunning = false;
            StopCoroutine(PatternOne());
            StartCoroutine(PatternTwo());
        }
        

        state = State.STAY;
    }
    protected override void DeathOn() {
        PoolManager.Instance.Despawn(gameObject);
        GameObject thisItem = PoolManager.Instance.Spawn(item.gameObject.name);
        thisItem.transform.position = this.gameObject.transform.position;
        GameManager.Instance.Point += 1000;
        GameManager.Instance.Score();
        GameManager.Instance.KillCount += 1;
        hpBar.transform.GetChild(0).gameObject.SetActive(false);
        hpBar.transform.GetChild(1).gameObject.SetActive(false);
        hpBar.transform.GetChild(2).gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }

    private IEnumerator PatternOne()
    {
        WaitForSeconds waitForSeconds;
        level[2].isRunning = true;
        while(level[2].isRunning)
        {
            waitForSeconds = new WaitForSeconds(2f);

            yield return waitForSeconds;

            //탄막 각도
            float angleStep = 0;
            //중심 각도
            float startAngle = 0;

            Vector3 directionToTarget;

            int patternNum = UnityEngine.Random.Range(1, 4);

            switch (patternNum)
            {
                case 1:
                    angleStep = -firstPattern.circlePattern.spreadAngle / (firstPattern.circlePattern.numberOfBullets - 1);
                    waitForSeconds = new WaitForSeconds(0.3f);
                    for (int i = 0; i < 4; i++)
                    {
                        directionToTarget = (target.transform.position - transform.position).normalized;
                        startAngle = Vector3.SignedAngle(-transform.right, directionToTarget, Vector3.forward)
                            - (-firstPattern.circlePattern.spreadAngle / firstPattern.circlePattern.midAngle);
                        for (int j = 0; j < firstPattern.circlePattern.numberOfBullets; j++)
                        {
                            float angle = startAngle + j * angleStep;
                            Vector3 direction = Quaternion.Euler(0, 0, angle) * -transform.right;

                            GameObject bullet = PoolManager.Instance.Spawn(firstPattern.circlePattern.bulletPrefab.gameObject.name);
                            bullet.transform.position = transform.position;
                            bullet.transform.rotation = Quaternion.identity;
                            bullet.transform.rotation = Quaternion.Euler(0, 0, angle+90f);
                            enemyBullet = bullet.GetComponent<EnemyBullet>();
                            bullet.transform.position += direction;

                            enemyBullet.Direction = direction;
                        }
                        yield return waitForSeconds;
                    }
                    break;
                case 2:
                    for (int i = 0; i < firstPattern.targetPattern.numberOfBullets; i++)
                    {
                        waitForSeconds = new WaitForSeconds(UnityEngine.Random.Range(0.2f, 0.05f));
                        directionToTarget = (target.transform.position - transform.position);
                        float slopeAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;
                        GameObject bullet = PoolManager.Instance.Spawn(firstPattern.targetPattern.bulletPrefab.gameObject.name);
                        bullet.transform.position = transform.position;
                        bullet.transform.rotation = Quaternion.Euler(0, 0, slopeAngle);
                        enemyBullet = bullet.GetComponent<EnemyBullet>();

                        enemyBullet.Direction = directionToTarget.normalized;
                        yield return waitForSeconds;
                    }
                    break;
                case 3:
                    waitForSeconds = new WaitForSeconds(0.025f);
                    angleStep = -firstPattern.n_WayPattern.spreadAngle / (firstPattern.n_WayPattern.numberOfBullets - 1);
                    directionToTarget = (target.transform.position - transform.position).normalized;
                    startAngle = Vector3.SignedAngle(-transform.right, directionToTarget, Vector3.forward)
                        - (-firstPattern.n_WayPattern.spreadAngle / 2);
                    for (int i = 0; i < firstPattern.n_WayPattern.numberOfBullets; i++)
                    {
                        float angle = startAngle + i * angleStep;
                        Vector3 direction = Quaternion.Euler(0, 0, angle) * -transform.right;

                        GameObject bullet = PoolManager.Instance.Spawn(firstPattern.n_WayPattern.bulletPrefab.gameObject.name);
                        bullet.transform.position = transform.position;
                        bullet.transform.rotation = Quaternion.identity;
                        enemyBullet = bullet.GetComponent<EnemyBullet>();
                        bullet.transform.position += direction;

                        enemyBullet.Direction = direction;
                        yield return waitForSeconds;
                    }
                    break;
            }
        }
        
    }

    private IEnumerator PatternTwo()
    {
        WaitForSeconds waitForSeconds;
        level[1].isRunning = true;
        while (level[1].isRunning)
        {
            waitForSeconds = new WaitForSeconds(2f);

            yield return waitForSeconds;

            //탄막 각도
            float angleStep = 0;
            //중심 각도
            float startAngle = 0;

            float angle;

            angleStep = -secondPattern.n_WayPatterns[0].spreadAngle / (secondPattern.n_WayPatterns[0].numberOfBullets - 1);
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            startAngle = Vector3.SignedAngle(-transform.right, directionToTarget, Vector3.forward)
                - (-secondPattern.n_WayPatterns[0].spreadAngle);
            for (int i = 0; i < secondPattern.n_WayPatterns[0].numberOfBullets; i++)
            {
                angle = startAngle + i * angleStep;
                Vector3 direction = Quaternion.Euler(0, 0, angle) * -transform.right;

                GameObject bullet = PoolManager.Instance.Spawn(secondPattern.n_WayPatterns[0].bulletPrefab.gameObject.name);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;
                enemyBullet = bullet.GetComponent<EnemyBullet>();
                bullet.transform.position += direction;

                enemyBullet.Direction = direction;
            }
            waitForSeconds = new WaitForSeconds(0.5f);

            yield return waitForSeconds;

            angleStep = -secondPattern.n_WayPatterns[1].spreadAngle / (secondPattern.n_WayPatterns[1].numberOfBullets - 1);
            directionToTarget = (target.transform.position - transform.position).normalized;
            startAngle = Vector3.SignedAngle(-transform.right, directionToTarget, Vector3.forward)
                - (-secondPattern.n_WayPatterns[1].spreadAngle);
            for (int i = 0; i < secondPattern.n_WayPatterns[1].numberOfBullets; i++)
            {
                angle = startAngle + i * angleStep;
                Vector3 direction = Quaternion.Euler(0, 0, angle) * -transform.right;

                GameObject bullet = PoolManager.Instance.Spawn(secondPattern.n_WayPatterns[1].bulletPrefab.gameObject.name);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;
                enemyBullet = bullet.GetComponent<EnemyBullet>();
                bullet.transform.position += direction;

                enemyBullet.Direction = direction;
            }
        }
       
    }

    private IEnumerator PatternThree()
    {
        WaitForSeconds waitForSeconds;
        level[0].isRunning = true;
        //탄막 각도
        float angleStep = 0;
        //중심 각도
        float startAngle = 0;

        Vector3 directionToTarget;

       
        StartCoroutine(RaserPattern());
        waitForSeconds = new WaitForSeconds(0.15f);
        for (int i=0;i<8;i++)
        {
            angleStep = -thirdPattern.n_WayPattern.spreadAngle / (thirdPattern.n_WayPattern.numberOfBullets - 1);
            directionToTarget = (target.transform.position - transform.position).normalized;
            startAngle = Vector3.SignedAngle(-transform.right, directionToTarget, Vector3.forward)
                - (-thirdPattern.n_WayPattern.spreadAngle / 2);
            for (int j = 0; j < thirdPattern.n_WayPattern.numberOfBullets; j++)
            {
                float angle = startAngle + j * angleStep;
                Vector3 direction = Quaternion.Euler(0, 0, angle) * -transform.right;

                GameObject bullet = PoolManager.Instance.Spawn(thirdPattern.n_WayPattern.bulletPrefab.gameObject.name);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;
                enemyBullet = bullet.GetComponent<EnemyBullet>();
                bullet.transform.position += direction;

                enemyBullet.Direction = direction;
            }
            yield return waitForSeconds;
        }
    }

    private IEnumerator RaserPattern()
    {
        WaitForSeconds waitForSeconds;
        waitForSeconds = new WaitForSeconds(0.4f);
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

        for (int i = 0; i < 4; i++)
        {
            directionToTarget = (target.transform.position - transform.position);

            // 아크탄젠트를 사용하여 각도를 계산
            float slopeAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg + 90f;
            GameObject bullet = PoolManager.Instance.Spawn(thirdPattern.raserBullet.bulletPrefab.gameObject.name);
            bullet.transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = true;
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;
            bullet.transform.rotation *= Quaternion.Euler(0, 0, slopeAngle);
            StartCoroutine(RaserSub(bullet));
            yield return waitForSeconds;
        }
        StopCoroutine(RaserPattern());
    }

    private IEnumerator RaserSub(GameObject bullet)
    {
        WaitForSeconds waitForSeconds;
        waitForSeconds = new WaitForSeconds(2f);
        yield return waitForSeconds;

        waitForSeconds = new WaitForSeconds(0.2f);
        bullet.transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
        for (int i = 0; i < 4; i++)
        {
            bullet.transform.GetChild(0).transform.localScale += new Vector3(1f, 0f, 0f);
            yield return waitForSeconds;
        }
        waitForSeconds = new WaitForSeconds(0.1f);
        for (int i = 0; i < 4; i++)
        {
            bullet.transform.GetChild(0).transform.localScale -= new Vector3(1f, 0f, 0f);
            yield return waitForSeconds;
        }
        bullet.transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = true;
        PoolManager.Instance.Despawn(bullet.gameObject);
        StopCoroutine(RaserSub(bullet));
    }


    public override void Damaged(float damage)
    {
        if (save)
            return;

        Hp -= damage;
        hpBar.GetComponent<BossHpBar>().CurHp = Hp;
        if (Hp <= 0)
        {
            StopCoroutine(PatternOne());
            StopCoroutine(PatternTwo());
            StopCoroutine(PatternThree());
            HPCount -= 1;
            if(HPCount<0)
            {
                state = State.DEATH;
                return;
            }
            Hp = level[HPCount].HP;
            hpBar.GetComponent<BossHpBar>().BossHpCount(HPCount);
            hpBar.GetComponent<BossHpBar>().MaxHp = Hp;
            hpBar.GetComponent<BossHpBar>().CurHp = Hp;
            save = true;
            StartCoroutine(SaveTime());
        }   
    }

    private IEnumerator SaveTime()
    {
        yield return new WaitForSeconds(5f);
        save = false;
    }
}
