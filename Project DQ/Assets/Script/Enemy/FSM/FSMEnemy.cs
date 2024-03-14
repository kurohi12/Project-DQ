using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 유니티 UI

public enum State
{
    INIT,
    STAY,
    MOVE,
    ATTACK,
    GOAL,
    DEATH,
    NULL
}

public abstract class FSMEnemy : MonoBehaviour
{
    [SerializeField]
    public float Speed = 3; //움직이는 속도
    [SerializeField]
    public float Hp = 10; //체력
    [SerializeField]
    public List<WayPoints> wayPoints = new List<WayPoints>(); //베지어 곡선 좌표
    [SerializeField]
    protected GameObject item = null; //드롭 아이템

    protected Vector3 bPosition;
    protected float t;
    protected int nextMove = 0;
    public Text scoreText;

  


    private WaitForEndOfFrame moveFrame = new WaitForEndOfFrame();

    public GameObject Item { get { return (GameObject)item; } set { item = (GameObject)value; } }

    protected State state = State.STAY;

    private void Update()
    {
        switch (state)
        {
            case State.INIT:
                Init();
                break;
            case State.STAY:
                StayOn();
                break;
            case State.MOVE:
                MoveOn();
                break;
            case State.ATTACK:
                AttackOn();
                break;
            case State.GOAL:
                GoalOn();
                break;
            case State.DEATH:
                DeathOn();
                break;
            case State.NULL:
                break;
        }
    }

    protected virtual void Init() { }
    protected virtual void StayOn() { }
    protected virtual void MoveOn() { }
    protected virtual void GoalOn() { }
    protected virtual void AttackOn() { }
    protected virtual void DeathOn() { }

    private void OnEnable()
    {
        state = State.INIT;
    }

    private void OnDisable()
    {
        state = State.INIT;
    }

    protected IEnumerator MoveRoutine()
    {
        t += Time.deltaTime * Speed;

        while (t < 1)
        {
            if (nextMove < wayPoints.Count)
            {
                bPosition = BezieCurve(wayPoints[nextMove].way.Length);

                transform.position = bPosition;
            }

            yield return moveFrame;
        }

        state = State.GOAL;
    }

    private Vector3 BezieCurve(int size)
    {
        Vector3 curve = Vector3.zero;
        switch (size)
        {
            case 2:
                {
                    curve = (1 - t) * wayPoints[nextMove].way[0]
                    + t * wayPoints[nextMove].way[1];
                    break;
                }
            case 3:
                {
                    curve = Mathf.Pow(1 - t, 2) * wayPoints[nextMove].way[0]
                    + 2 * t * (1 - t) * wayPoints[nextMove].way[1]
                    + Mathf.Pow(t, 2) * wayPoints[nextMove].way[2];
                    break;
                }
            case 4:
                {
                    curve = Mathf.Pow(1 - t, 3) * wayPoints[nextMove].way[0]
                    + 3 * t * Mathf.Pow(1 - t, 2) * wayPoints[nextMove].way[1]
                    + 3 * t * (1 - t) * wayPoints[nextMove].way[2]
                    + Mathf.Pow(t, 3) * wayPoints[nextMove].way[3];
                    break;
                }
        }

        return curve;
    }

    public virtual void Damaged(float damage)
    {
    }

    protected virtual void DeadSet()
    {
        PoolManager.Instance.Spawn(item.gameObject.name);
        item.transform.position = this.gameObject.transform.position;
        GameManager.Instance.Point += 10;
        scoreText.text = string.Format("{0:n0}", GameManager.Instance.Point);
        GameManager.Instance.KillCount += 1;
    }
}
