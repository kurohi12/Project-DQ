using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private void Start()
    {
        
    }
    void Update()
    {
        Fire();
    }

    void Fire()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }
}
