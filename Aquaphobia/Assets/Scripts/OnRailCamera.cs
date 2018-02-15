using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRailCamera : MonoBehaviour {

    public GameObject dot;

    public Transform[] railPoints;
    public float speed;
    float posOnTrack = -1f;   // full track
    float posOnRail;    // piece of track

    private void Update()
    {
        if (posOnTrack < 0 || posOnTrack >= railPoints.Length - 1)
        {
            posOnTrack += Time.deltaTime * speed;
            Debug.Log("Out of bounds.");
            return;
        }

        transform.position = Vector3.Slerp(
            railPoints[(int)posOnTrack].position,
            railPoints[(int)posOnTrack + 1].position,
            CalculatePos());

        transform.rotation = Quaternion.Slerp(
            railPoints[(int)posOnTrack].rotation,
            railPoints[(int)posOnTrack + 1].rotation,
            CalculatePos());

        posOnTrack += Time.deltaTime * speed;

        //Instantiate(dot, transform.position, transform.rotation);
        
    }

    private float CalculatePos()
    {
        posOnRail = posOnTrack;

        while (posOnRail > 1)
        {
            posOnRail--;
        }

        return posOnRail;
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i<railPoints.Length; i++)
        {
            Gizmos.color = new Vector4(255, 0, 0, 0.3f);
            Gizmos.DrawCube(railPoints[i].position,Vector3.one*0.3f);
            Gizmos.DrawLine(railPoints[i].position, railPoints[i].position+railPoints[i].forward*0.5f);
        }
    }
}
