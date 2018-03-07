using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour {

    private Checkpoint[] _checkpoints;
    private Checkpoint _activeCheckpoint;
    private BoxDown[] _boxDowns;

    public float resetHeight;

    private void Start()
    {
        _checkpoints = FindObjectsOfType<Checkpoint>();
        _boxDowns = FindObjectsOfType<BoxDown>();
    }

    private void Update()
    {
        if (transform.position.y < resetHeight)
        {
            transform.position = _activeCheckpoint.checkpointPosition;
            transform.rotation = _activeCheckpoint.checkpointRotation;

            foreach (BoxDown boxDown in _boxDowns)
            {
                boxDown.BoxReset();
            }
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
