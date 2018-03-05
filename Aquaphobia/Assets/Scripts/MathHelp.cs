using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelp {

    public static Vector3 GetCurvePosition(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
    }

    public static Quaternion GetCurveRotation(Quaternion q0, Quaternion q1, Quaternion q2, float t)
    {
        return Quaternion.Lerp(Quaternion.Lerp(q0, q1, t), Quaternion.Lerp(q1, q2, t), t);
    }

    /// <summary>
    /// Takes two pairs of vector3 and calculates pairs direction.
    /// </summary>
    /// <param name="v0">First pairs start.</param>
    /// <param name="v1">First pairs end.</param>
    /// <param name="w0">Second pairs start.</param>
    /// <param name="w1">Second pairs end.</param>
    /// <returns>Angle between directions in degrees.</returns>
    public static float AngleBetweenVector3(Vector3 v0, Vector3 v1, Vector3 w0, Vector3 w1)
    {
        Vector3 v = v0 - v1;
        Vector3 w = w0 - w1;

        return Vector3.Angle(v, w);
    }

    /// <summary>
    /// Takes three vector3 and calculates direction.
    /// </summary>
    /// <param name="v0">First direction start.</param>
    /// <param name="v1">First direction end and second direction start.</param>
    /// <param name="v2">Second direction end.</param>
    /// <returns>Angle between directions in degrees.</returns>
    public static float AngleBetweenVector3(Vector3 v0, Vector3 v1, Vector3 v2)
    {
        Vector3 v = v0 - v1;
        Vector3 w = v1 - v2;

        return Vector3.Angle(v, w);
    }
}
