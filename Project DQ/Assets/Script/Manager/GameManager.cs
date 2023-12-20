using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private float curTime;
    [SerializeField]
    private float playerPoint;

    public float GameTime
    {
        get { return curTime; }
        set { curTime = value; }
    }

    public float Point
    {
        get { return playerPoint; }
        set { playerPoint = value; }
    }


    // Start is called before the first frame update
    public void Start()
    {
        curTime = 0;
        playerPoint = 0;
    }

    // Update is called once per frame
    public void Update()
    {
        curTime += Time.deltaTime;
    }
}
