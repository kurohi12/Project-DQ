using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object : MonoBehaviour
{
    public Vector2 SpawnPoint = Vector2.zero;
    public Vector2 DespawnPoint = Vector2.zero;
    public float LostTime;
    protected float curTime;
    public int ObjectType; //юс╫ц 1,2,3

    public void OnEnable()
    {
        gameObject.transform.position = SpawnPoint;
    }

    public abstract void Attack();

    private void OnCollisionEnter(Collision collision)
    {
        Attack();
        PoolManager.Instance.Despawn(gameObject);
    }

    public void OnDisable()
    {
        gameObject.transform.position = DespawnPoint;
        curTime = 0;
    }

}
