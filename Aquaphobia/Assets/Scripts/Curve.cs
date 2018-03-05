using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour {

    public Transform start;
    public Transform middle;
    public Transform end;

    private void OnDrawGizmos()
    {
        float t = 0;
        Vector3 startPoint =
            MathHelp.GetCurvePosition(start.position, middle.position, end.position, t);
        Vector3 endPoint;

        while (t < 1)
        {
            t += 0.015625f; // 1 divided by 64
            endPoint =
                MathHelp.GetCurvePosition(start.position, middle.position, end.position, t);

            Debug.DrawLine(startPoint, endPoint, Color.black);

            startPoint = endPoint;
        }
    }
}
