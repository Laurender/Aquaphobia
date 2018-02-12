using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMaker : MonoBehaviour {

    public GameObject waterPrefab;
    public int gridSize;

    public float speed;

    private void Awake()
    {
        for (int x = -gridSize; x <= gridSize; x++)
        {
            for (int z = -gridSize; z <= gridSize; z++)
            {
                GameObject waterTile = Instantiate(waterPrefab, transform);
                waterTile.transform.position = new Vector3(x * 30, transform.position.y, z * 30);
            }
        }
    }

    private void Update()
    {
        Vector3 newPos = transform.position;
        newPos.y += speed * Time.deltaTime;
        transform.position = newPos;
    }
}
