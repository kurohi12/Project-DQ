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
    public float Speed = 3; //�����̴� �ӵ�
    [SerializeField]
    public float Hp = 10; //ü��
    [SerializeField]
    public List<WayPoints> wayPoints = new List<WayPoints>(); //������ � ��ǥ
}
