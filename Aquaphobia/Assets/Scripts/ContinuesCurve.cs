using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuesCurve : MonoBehaviour {

    public Transform[] curvePoints;
    public float angleTolerance = 1;

    private bool initDone = false;
    
    private void OnDrawGizmos()
    {
        if (!initDone)
        {
            InitCurvePoints();
        }

        Color color;

        for (int i = 0; i < curvePoints.Length - 1; i += 2)
        {
            // draw curve
            {
                color = Color.black;

                Vector3 start = curvePoints[i].position;
                float t = 0f;

                while (t < 1)
                {
                    t += 0.015625f; // 1 divided by 64

                    Vector3 end = MathHelp.GetCurvePosition(
                        curvePoints[i].position,
                        curvePoints[i + 1].position,
                        curvePoints[i + 2].position,
                        t);

                    Debug.DrawLine(start, end, color);

                    start = end;
                }
            }

            // draw straight lines
            {
                if (i == 0)
                {   // first line no angle check needed
                    color = Color.white;
                    Debug.DrawLine(curvePoints[i].position, curvePoints[i + 1].position, color);
                    continue;
                }
                else if (i == curvePoints.Length - 1)
                {   // last line no angle check needed
                    color = Color.white;
                    Debug.DrawLine(curvePoints[i - 1].position, curvePoints[i].position, color);
                    continue;
                }
                else
                {   // check angle and set correct color
                    float angle = MathHelp.AngleBetweenVector3(
                        curvePoints[i - 1].position,
                        curvePoints[i].position,
                        curvePoints[i + 1].position);

                    if (angle > -angleTolerance && angle < angleTolerance)
                    {
                        color = Color.white;
                    }
                    else
                    {
                        color = Color.yellow;
                    }

                    Debug.DrawLine(curvePoints[i - 1].position, curvePoints[i].position, color);
                    Debug.DrawLine(curvePoints[i].position, curvePoints[i + 1].position, color);
                }
            }
        }
    }

    private void InitCurvePoints()
    {
        if (!initDone)
        {
            curvePoints = new Transform[transform.childCount];
            for (int i = 0; i < curvePoints.Length; i++)
            {
                curvePoints[i] = transform.GetChild(i);
            }
            initDone = true;
        }
    }
}
