using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Way
{
    x1,
    y1, 
    x2, 
    y2, 
    x3, 
    y3, 
    x4, 
    y4
}

public class E_Enemy : MonoBehaviour
{
    [SerializeField]
    public float Speed = 3; //움직이는 속도
    [SerializeField]
    public float Hp = 10; //체력
    [SerializeField]
    public List<WayPoints> wayPoints = new List<WayPoints>(); //베지어 곡선 좌표
}
