using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    
    public Transform Target;
    public Transform Pivot;

    public Vector3 offSet;

    public bool useOffsetValues;
    public bool InvertY = false;

    public float rotateSpeed;

    public float maxViewAngle;
    public float minViewAngle;

	// Use this for initialization
	void Start () {
        if (!useOffsetValues)
        {
            offSet = Target.position - transform.position;
        }
        Pivot.transform.position = Target.transform.position;
        Pivot.transform.parent = Target.transform;

        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void LateUpdate () {

        //Get the X position of the mouse and rotate the target
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        Target.Rotate(0f, horizontal, 0f);

        //Get the Y Position fo the mouse and rotate the pivot
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
        if (InvertY)
        {
            Pivot.Rotate(vertical, 0f, 0f);
        }
        else
        {
            Pivot.Rotate(-vertical, 0f, 0f);
        }

        //Limit up/down rotation
        if(Pivot.rotation.eulerAngles.x > maxViewAngle && Pivot.rotation.eulerAngles.x < 180f)
        {
            Pivot.rotation = Quaternion.Euler(maxViewAngle, 0, 0);
        }

        if(Pivot.rotation.eulerAngles.x > 180 && Pivot.rotation.eulerAngles.x < 360 + minViewAngle)
        {
            Pivot.rotation = Quaternion.Euler(360+minViewAngle, 0, 0);
        }

        //Move the camera based on the current rotation of the target and the original offset
        float desiredYAngle = Target.eulerAngles.y;
        float desiredXAngle = Pivot.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = Target.position - (rotation * offSet);

        //transform.position = Target.position - offSet;

        transform.LookAt(Target);
	}
}
