using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LBullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private Vector3 direction = Vector3.zero;
    public int BulletType = 0;

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public Vector3 Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
