using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Item : MonoBehaviour
{

    private int speed = 2;

    //아이템 코드 0=포인트 , 1=파워 레벨
    private int code = 0;

    private float status = 0;

    public int Code
    {
        get { return code; }
        set { code = value; }
    }

    public float Status
    {
        get { return status; }
        set { status = value; }
    }



    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        transform.position += -Vector3.right * speed * Time.deltaTime;
    }
}
