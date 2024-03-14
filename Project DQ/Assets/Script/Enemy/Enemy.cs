using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    [SerializeField]
    public float Speed = 3; //움직이는 속도
    [SerializeField]
    public float Hp = 10; //체력
    [SerializeField]
    public List<WayPoints> WayPoints = new List<WayPoints>(); //베지어 곡선 좌표
    [SerializeField]
    protected GameObject item = null; //드롭 아이템
    [SerializeField]
    public bool Setting=false;

    protected float curTime=0;
    [SerializeField]
    protected float[] stopTime;

    protected int stopCount=0;

    [SerializeField]
    protected bool goalIn=false;
    protected bool coroutineRun = true;

    protected float t;

    [SerializeField]
    protected int nextMove = 0;

    protected Vector3 bPosition;

    private WaitForEndOfFrame moveFrame = new WaitForEndOfFrame();

    public GameObject Item { get { return (GameObject)item; } set { item = (GameObject)value; } }

    protected void OnEnable()
    {
        
    }

    protected void OnDisable()
    {
        goalIn = false;
        t = 0;
        Setting = false;
        nextMove = 0;
        curTime = 0;
    }

    public void Damaged(float damage)
    {
        Hp -= damage;
        if (Hp <= 0)
            DeadSet();
    }

    protected IEnumerator MoveRoutine()
    {
        if (nextMove >= WayPoints.Count)
        {
            PoolManager.Instance.Despawn(gameObject);
        }

        t += Time.deltaTime * Speed;

        while (t < 1)
        {
            if(nextMove < WayPoints.Count)
            {
                bPosition = BezieCurve(WayPoints[nextMove].way.Length);

                transform.position = bPosition;
            }

            yield return moveFrame;
        }

        goalIn = true;
    }

    private Vector3 BezieCurve(int size)
    {
        Vector3 curve = Vector3.zero;
        switch (size)
        {
            case 2:
                {
                    curve = (1 - t) * WayPoints[nextMove].way[0]
                    + t * WayPoints[nextMove].way[1];
                    break;
                }
            case 3:
                {
                    curve = Mathf.Pow(1 - t,2) * WayPoints[nextMove].way[0]
                    + 2 * t * (1 - t) * WayPoints[nextMove].way[1]
                    + Mathf.Pow(t,2) * WayPoints[nextMove].way[2];
                    break;
                }
            case 4:
                {
                    curve = Mathf.Pow(1 - t, 3) * WayPoints[nextMove].way[0]
                    + 3 * t * Mathf.Pow(1 - t, 2) * WayPoints[nextMove].way[1]
                    + 3 * t * (1 - t) * WayPoints[nextMove].way[2]
                    + Mathf.Pow(t, 3) * WayPoints[nextMove].way[3];
                    break;
                }
        }

        return curve;
    }

    private void DeadSet()
    {
        if(item!=null)
            PoolManager.Instance.Spawn(item.gameObject.name);
        GameManager.Instance.Point += 10;
        PoolManager.Instance.Despawn(this.gameObject);
    }
}
