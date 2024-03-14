using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : Enemy
{
    private void Update()
    {
        if (goalIn)
        {
            t = 0;
            StopCoroutine(MoveRoutine());
            nextMove++;
            coroutineRun = false;
            goalIn = false;
        }


        if (!coroutineRun)
        {
            if (stopCount < stopTime.Length)
            {
                curTime += Time.deltaTime;
                if (curTime > stopTime[stopCount])
                    coroutineRun = true;
            }
            else coroutineRun = true;
        }

        if (!goalIn && coroutineRun)
            StartCoroutine(MoveRoutine());
    }
}
