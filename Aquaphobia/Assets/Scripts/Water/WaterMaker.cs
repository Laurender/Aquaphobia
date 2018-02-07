using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMaker : MonoBehaviour {

    public GameObject waterPrefab;
    public int gridSize;

    private void Awake()
    {
        for (int x = -gridSize; x < gridSize; x++)
        {
            for (int z = -gridSize; z < gridSize; z++)
            {
                GameObject waterTile = Instantiate(waterPrefab, transform);
                waterTile.transform.position = new Vector3(x * 10, 0, z * 10);
            }
        }
    }
}
