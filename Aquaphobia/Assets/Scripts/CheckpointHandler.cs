using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour {

    private Checkpoint[] _checkpoints;
    private Checkpoint _activeCheckpoint;

    public float resetHeight;

    private void Awake()
    {
        _checkpoints = FindObjectsOfType<Checkpoint>();
    }

    private void Update()
    {
        if (transform.position.y < resetHeight)
        {
            transform.position = _activeCheckpoint.checkpointPosition;
            transform.rotation = _activeCheckpoint.checkpointRotation;
        }
    }

    public void SetActiveCheckpoint(Checkpoint activeCheckpoint)
    {
        foreach (Checkpoint checkpoint in _checkpoints)
        {
            checkpoint.activeCheckpoint = false;
        }

        activeCheckpoint.activeCheckpoint = true;
        _activeCheckpoint = activeCheckpoint;
    }
}
