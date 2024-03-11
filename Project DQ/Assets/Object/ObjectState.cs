using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아래 코드는 사용예시로 각 개체의 HP나 움직임을 불러와서 사용해야함
public class ObjectState : MonoBehaviour
{
    public int States = 0;
    public float Hp = 10;
    public float Damage = 1;
    private bool coroutineRun=false;
    private bool top = false;
    private int moveCnt = 0;
    public bool moveStop =false;

    private void Start()
    {
        StartCoroutine(Move());
    }

    private void Update()
    {
        if (States == 1&&!coroutineRun)
        {
            StartCoroutine(FireState());
            States = 0;
        }else if (States == 2 && !coroutineRun)
        {
            StartCoroutine(IceState());
            States = 0;
        }else if(States == 3 && !coroutineRun)
        {
            StartCoroutine(ExState());
            States = 0;
        }


        if (moveCnt >= 3) top = false;
        else if (moveCnt <= 0) top = true;



    }

    private IEnumerator Move()
    {
        while(true)
        {
            if(!moveStop)
            {
                if (top)
                {
                    gameObject.transform.position -= Vector3.up;
                    moveCnt++;
                }
                else if (!top)
                {
                    gameObject.transform.position += Vector3.up;
                    moveCnt--;
                }
            }
            yield return new WaitForSeconds(1f);
        }
        
        
    }

    private IEnumerator IceState()
    {
        coroutineRun = true;
        if (!moveStop)
        {
            moveStop = true;
            yield return new WaitForSeconds(2f);
            moveStop = false;
        }
        States = 0;
        coroutineRun = false;
        yield return null;
    }
    private IEnumerator FireState()
    {
        coroutineRun = true;
        for (int i = 0; i < 5; i++)
        {
            Hp -= Damage;
            yield return new WaitForSeconds(1f);
        }
        States = 0;
        coroutineRun=false;
        yield return null;
    }

    private IEnumerator ExState()
    {
        if(coroutineRun==false) yield return null;
        coroutineRun = true;
        Hp -= 8;
        States = 0;
        yield return new WaitForSeconds(2f);
        coroutineRun = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<LBullet>().BulletType==1)
        {
            States = 1;
        }else if (collision.gameObject.GetComponent<LBullet>().BulletType == 2)
        {
            States = 2;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponent<LBullet>().BulletType == 3&&!coroutineRun)
        {
            States = 3;
        }
    }
}
