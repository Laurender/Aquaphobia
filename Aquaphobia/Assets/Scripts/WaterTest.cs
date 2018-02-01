using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTest : MonoBehaviour {

	public GameObject prefab;
	GameObject[,] cubes;
	float angle;
	float movement;
	public float speed;
	public bool reverse;
	public bool freeze;

	// Use this for initialization
	void Start () {
		cubes = new GameObject[10, 10];
		for (int i = 0; i < 10; i++) {
			for (int j = 0; j < 10; j++) {
				Vector3 pos = new Vector3 (i, 0, j);
				pos += transform.position;
				cubes[i,j] = Instantiate (prefab, pos, Quaternion.identity);
				cubes [i, j].transform.parent = transform;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!freeze) {
			angle += Time.deltaTime;
			movement = Mathf.Sin (angle);
			if (reverse) {
				movement *= -1f;
			}
			foreach (GameObject cube in cubes) {
				Vector3 position = cube.transform.localPosition;
				position.y = movement * (position.x - 4.5f) * (position.z - 4.5f) * speed;
				cube.transform.localPosition = position;
			}
		}
	}
}
