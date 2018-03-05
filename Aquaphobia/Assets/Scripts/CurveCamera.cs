using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveCamera : MonoBehaviour {

    public Curve[] cameraPath;
    public float distanceTolerance = 0.1f;
    public float angleTolerance = 1;

    private void OnDrawGizmos()
    {
        Color color = Color.black;

        if (cameraPath.Length < 1)
        {
            return;
        }

        for (int i = 0; i < cameraPath.Length; i++)
        {
            if (i == 0)
            {   // just draw first half
                color = Color.white;
                Debug.DrawLine(cameraPath[i].start.position, cameraPath[i].middle.position, color);
                continue;
            }
            else if (i == cameraPath.Length - 1)
            {   // draw end 
                color = Color.white;
                Debug.DrawLine(cameraPath[i].middle.position, cameraPath[i].end.position, color);
            }

            // draw curve intersections
            float angle = MathHelp.AngleBetweenVector3(
                cameraPath[i - 1].middle.position,
                cameraPath[i - 1].end.position,
                cameraPath[i].start.position,
                cameraPath[i].middle.position);

            float distance = 
                Vector3.Distance(cameraPath[i - 1].end.position, cameraPath[i].start.position);
            if (distance > distanceTolerance)
            {
                color = Color.red;
            }
            else if (angle > -1 * angleTolerance && angle < angleTolerance)
            {
                color = Color.white;
            }
            else
            {
                color = Color.yellow;
            }

            Debug.DrawLine(cameraPath[i - 1].middle.position, cameraPath[i - 1].end.position, color);
            Debug.DrawLine(cameraPath[i].start.position, cameraPath[i].middle.position, color);
        }
    }
}
