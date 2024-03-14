using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwo : Enemy
{
    private void Update()
    {
        if (goalIn)
        {
            t = 0;
            StopCoroutine(MoveRoutine());
            if(nextMove==0)
                gameObject.GetComponent<CircleBullet>().Shoot();
            nextMove++;
            coroutineRun = false;
            goalIn = false;
        }

        
        if (!coroutineRun)
        {
            if(stopCount<stopTime.Length)
            {
                curTime += Time.deltaTime;
                if (curTime > stopTime[stopCount])
                    coroutineRun = true;
            }else coroutineRun = true;
        }

        if(!goalIn&&coroutineRun)
            StartCoroutine(MoveRoutine());
    }
}
