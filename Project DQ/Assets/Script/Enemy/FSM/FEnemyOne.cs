using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FEnemyOne : FSMEnemy
{
    private float hp;
    protected override void Init() {
        hp = Hp;
        t = 0;
        state = State.MOVE;
    }

    protected override void MoveOn() {
        StartCoroutine(MoveRoutine());
    }

    protected override void GoalOn() {
        StopCoroutine(MoveRoutine());
        PoolManager.Instance.Despawn(this.gameObject);
    }

    protected override void DeathOn() {
        PoolManager.Instance.Despawn(gameObject);
        GameObject thisItem = PoolManager.Instance.Spawn(item.gameObject.name);
        thisItem.transform.position = this.gameObject.transform.position;
        GameManager.Instance.Point += 10;
        GameManager.Instance.Score();
        GameManager.Instance.KillCount += 1;
    }
    public override void Damaged(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            state = State.DEATH;
        }

        if (hp > 0) state = State.MOVE;
    }
}
