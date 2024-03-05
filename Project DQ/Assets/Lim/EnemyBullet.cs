using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private Vector3 direction = Vector3.zero;

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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
