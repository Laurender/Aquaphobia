using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
	
	public GameObject prefab;
	GameObject[,] cubes;

	// Use this for initialization
	void Start () {
		cubes = new GameObject[10, 10];
		for (int i = 0; i < 10; i++) {
			for (int j = 0; j < 10; j++) {
				Vector3 pos = new Vector3 (i*10, 0, j*10);
				pos += transform.position;
				cubes[i,j] = Instantiate (prefab, pos, Quaternion.identity);
				cubes [i, j].transform.parent = transform;
				if((i+j)%2 == 1){
					cubes[i,j].GetComponent<WaterTest>().reverse = true;
				}
			}
		}
	}

}
