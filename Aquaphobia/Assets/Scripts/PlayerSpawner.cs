using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {

	public GameObject[] _spawners;

	public int _chosenSpawner;

	public GameObject _playerPrefab;

	public CameraController _myCC;

	// Use this for initialization
	void Awake () {
		
		_myCC.Target = Instantiate (_playerPrefab, _spawners [_chosenSpawner].transform.position, Quaternion.identity).transform;
		_myCC.Pivot = _myCC.Target.Find ("Cube");
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
