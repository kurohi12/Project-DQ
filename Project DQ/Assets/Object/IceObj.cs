using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceObj : Object
{
    public override void Attack()
    {
        gameObject.GetComponent<CircleBullet>().Shoot();
    }

    private void Update()
    {
        curTime += Time.deltaTime;
        if (LostTime < curTime)
        {
            PoolManager.Instance.Despawn(gameObject);
        }
    }
}
