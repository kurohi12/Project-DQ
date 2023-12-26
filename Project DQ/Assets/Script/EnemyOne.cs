using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : Enemy
{

    bool flag=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        flag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (flag)
        {
            StartCoroutine(BezierLining());
        }
        //transform.position += Direction * speed * Time.deltaTime;
    }

    private void OnDisable()
    {
        flag =false;
        t = 0;
    }
}
