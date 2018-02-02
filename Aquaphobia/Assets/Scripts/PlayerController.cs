using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

    private Vector3 _velocity;
    private Rigidbody _myRB;

	void Start () {
        _myRB = GetComponent<Rigidbody>();
	}

    public void Move(Vector3 velocity)
    {
        _velocity = velocity;
    }

    public void FixedUpdate()
    {
        _myRB.MovePosition(_myRB.position + _velocity * Time.fixedDeltaTime);
    }
}
