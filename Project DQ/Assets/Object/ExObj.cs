using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExObj : Object
{
    public GameObject exploObj;
    public override void Attack()
    {
        GameObject obj = Instantiate(exploObj);
        obj.transform.position = transform.position;
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
