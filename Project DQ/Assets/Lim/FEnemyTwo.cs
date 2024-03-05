using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FEnemyTwo : FSMEnemy
{
    private float hp;
    private float stayTime = 0.0f;

    [SerializeField]
    private float[] moveTime;
    private int Count=0;
    protected override void Init()
    {
        hp = Hp;
        Count = 0;
        stayTime = 0.0f;
        nextMove = 0;
        t = 0;
        state = State.MOVE;
    }
    protected override void StayOn()
    {
        if (moveTime.Length <= Count)
        {
            PoolManager.Instance.Despawn(this.gameObject);
            return;
        }

        stayTime += Time.deltaTime;

        if (stayTime > moveTime[Count])
        {
            t = 0;
            nextMove++;
            Count++;
            state = State.MOVE;
        }

        
    }
    protected override void MoveOn()
    {
        StartCoroutine(MoveRoutine());
    }
    protected override void GoalOn()
    {
        StopCoroutine(MoveRoutine());

        if (wayPoints.Count-1 <= nextMove)
            state = State.STAY;
        else state = State.ATTACK;
    }
    protected override void AttackOn()
    {
        gameObject.GetComponent<CircleBullet>().Shoot();

        state = State.STAY;
    }

    public override void Damaged(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            state = State.DEATH;
        }

    }

    protected override void DeathOn()
    {
        PoolManager.Instance.Despawn(gameObject);
        GameObject thisItem = PoolManager.Instance.Spawn(item.gameObject.name);
        thisItem.transform.position = this.gameObject.transform.position;
        GameManager.Instance.Point += 10;
        GameManager.Instance.Score();
        GameManager.Instance.KillCount += 1;

    }
}
