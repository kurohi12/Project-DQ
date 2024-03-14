using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointItem : Item
{
    private float point;

    public float Point
    {
        get { return point; }
        set { point = value; }
    }

    private void OnEnable()
    {
        point = Status;
    }
}
