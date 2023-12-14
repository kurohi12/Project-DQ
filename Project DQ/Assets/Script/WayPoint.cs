using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public Transform[] wayPoints = new Transform[4];
    private Vector2 gizmoPosition;
    private void OnDrawGizmos()
    {
        if (wayPoints[0] == null)
            return;

        for (float t = 0; t < 1; t += 0.05f)
        {

            gizmoPosition =
                +Mathf.Pow(1 - t, 3) * wayPoints[0].position
                + 3 * t * Mathf.Pow(1 - t, 2) * wayPoints[1].position
                + 3 * t * (1 - t) * wayPoints[2].position
                + Mathf.Pow(t, 3) * wayPoints[3].position;

            Gizmos.DrawSphere(gizmoPosition, 0.5f);

            Gizmos.DrawLine(new Vector2(wayPoints[0].position.x, wayPoints[0].position.y),
                new Vector2(wayPoints[1].position.x, wayPoints[1].position.y));
            Gizmos.DrawLine(new Vector2(wayPoints[2].position.x, wayPoints[2].position.y),
                new Vector2(wayPoints[3].position.x, wayPoints[3].position.y));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
