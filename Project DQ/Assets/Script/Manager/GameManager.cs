using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private float curTime;
    [SerializeField]
    private int playerPoint;
    [SerializeField]
    private Transform playerTransform;

    public float GameTime
    {
        get { return curTime; }
        set { curTime = value; }
    }

    public int Point
    {
        get { return playerPoint; }
        set { playerPoint = value; }
    }


    // Start is called before the first frame update
    private void Start()
    {
        curTime = 0;
        playerPoint = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        curTime += Time.deltaTime;

        
    }
}
