using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public Transform[] wayPoints = new Transform[4];
    public int size = 4;
    private Vector2 gizmoPosition;
    private void OnDrawGizmos()
    {
        if (wayPoints[0] == null)
            return;

        for (float t = 0; t < 1; t += 0.05f)
        {

            gizmoPosition = BezieCurve(size,t);

            Gizmos.DrawSphere(gizmoPosition, 0.25f);
        }
    }

    private Vector2 BezieCurve(int size,float t)
    {
        Vector3 curve = Vector3.zero;
        switch (size)
        {
            case 2:
                {
                    curve = (1 - t) * wayPoints[0].position
                    + t * wayPoints[1].position;
                    Gizmos.DrawLine(new Vector2(wayPoints[0].position.x, wayPoints[0].position.y),
                    new Vector2(wayPoints[1].position.x, wayPoints[1].position.y));
                    break;
                }
            case 3:
                {
                    curve = Mathf.Pow(1 - t, 2) * wayPoints[0].position
                    + 2 * t * (1 - t) * wayPoints[1].position
                    + Mathf.Pow(t, 2) * wayPoints[2].position;
                    Gizmos.DrawLine(new Vector2(wayPoints[0].position.x, wayPoints[0].position.y),
                    new Vector2(wayPoints[1].position.x, wayPoints[1].position.y));
                    Gizmos.DrawLine(new Vector2(wayPoints[2].position.x, wayPoints[2].position.y),
                       new Vector2(wayPoints[1].position.x, wayPoints[1].position.y));
                    break;
                }
            case 4:
                {
                    curve = Mathf.Pow(1 - t, 3) * wayPoints[0].position
                    + 3 * t * Mathf.Pow(1 - t, 2) * wayPoints[1].position
                    + 3 * t * (1 - t) * wayPoints[2].position
                    + Mathf.Pow(t, 3) * wayPoints[3].position;
                    Gizmos.DrawLine(new Vector2(wayPoints[0].position.x, wayPoints[0].position.y),
                    new Vector2(wayPoints[1].position.x, wayPoints[1].position.y));
                    Gizmos.DrawLine(new Vector2(wayPoints[2].position.x, wayPoints[2].position.y),
                    new Vector2(wayPoints[3].position.x, wayPoints[3].position.y));
                    break;
                }
        }

        return curve;
    }
}
