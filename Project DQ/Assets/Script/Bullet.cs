using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }
    //유도총알스크립트 하나 더 만들어서 프리펩에 추가 후 적용
}
