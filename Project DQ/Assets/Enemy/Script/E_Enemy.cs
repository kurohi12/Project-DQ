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

    
    private void Start()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Test");

        WayPoints wayPoint = new WayPoints();
        Speed = float.Parse(data[0]["Speed"].ToString());
        Hp = float.Parse(data[0]["Hp"].ToString());
        for (int i = 0; i < int.Parse(data[0]["wayPointCnt"].ToString()); i++)
        {
            string x = (E_Way.x1 + i).ToString();
            string y = (E_Way.y1 + i).ToString();
            wayPoint.way[i] = new Vector3(float.Parse(data[0][x].ToString()), float.Parse(data[0][y].ToString()), 0);
        }
        wayPoints.Add(wayPoint);
    }
}
