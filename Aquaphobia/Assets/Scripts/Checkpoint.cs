using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public bool activeCheckpoint;
    public Vector3 checkpointPosition;
    public Quaternion checkpointRotation;

    private CheckpointHandler checkpointHandler;

    private void Awake()
    {
        checkpointHandler = FindObjectOfType<CheckpointHandler>();
        checkpointPosition = transform.GetChild(0).position;
        checkpointRotation = transform.GetChild(0).rotation;
    }

    private void OnTriggerStay(Collider other)
    {
        if (activeCheckpoint)
        {
            return;
        }
        
        checkpointHandler.SetActiveCheckpoint(this);
    }
}
