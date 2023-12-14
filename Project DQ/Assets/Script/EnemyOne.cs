using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(BezierLining());
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += Direction * speed * Time.deltaTime;
    }
}
