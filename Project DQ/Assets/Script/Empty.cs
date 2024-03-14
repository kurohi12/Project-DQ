using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty : MonoBehaviour
{
    private NWayBullet nWayBullet;
    private CircleBullet circleBullet;

    // Start is called before the first frame update
    void Start()
    {
        nWayBullet = gameObject.GetComponent<NWayBullet>();
        circleBullet = gameObject.GetComponent<CircleBullet>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z)) 
        {
            nWayBullet.Shoot();    
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            circleBullet.Shoot();
        }
    }
}
