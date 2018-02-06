using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWaves : MonoBehaviour {

    Mesh mesh;

    public int waveTempo;
    float waveLength;

    public float waveHeight;

    public float xOffsetSpeed;
    float xOffset;
    public float yOffsetSpeed;
    float yOffset;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        waveLength = Mathf.PI * waveTempo;
    }

    private void Update()
    {
        xOffset += xOffsetSpeed * Time.deltaTime;
        yOffset += yOffsetSpeed * Time.deltaTime;

        Vector3[] newVertices = mesh.vertices;

        for (int i = 0; i < newVertices.Length; i++)
        {
            float sinX = Mathf.Sin((newVertices[i].x + xOffset) * waveLength);
            float sinY = Mathf.Sin((newVertices[i].y + yOffset) * waveLength);
            float zOffset = sinX + sinY;
            newVertices[i].z += zOffset * waveHeight;
        }

        mesh.vertices = newVertices;
    }
}
