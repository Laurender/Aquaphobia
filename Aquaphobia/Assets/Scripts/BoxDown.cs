using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDown : Curve {

    private float lerpTime;
    private float lerpFactor = 0f;
    private bool lerp = false;

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

    private void Update()
    {
        if (!lerp)
        {
            return;
        }

        if (lerp)
        {
            lerpFactor++;
            lerpTime += Time.deltaTime * lerpFactor * 0.125f;
            transform.position =
                MathHelp.GetCurvePosition(start.position, middle.position, end.position, lerpTime);
            transform.rotation =
                MathHelp.GetCurveRotation(start.rotation, middle.rotation, end.rotation, lerpTime);
        }

        if (lerpTime >= 1 && lerp)
        {
            lerp = false;
            transform.position = end.position;
            transform.rotation = end.rotation;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (lerp)
        {
            return;
        }

        if (other.gameObject.tag == "Player")
        {
            lerp = true;
        }
    }
}
