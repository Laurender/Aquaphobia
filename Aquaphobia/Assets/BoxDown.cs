using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDown : MonoBehaviour {

    public Transform end;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            transform.position = end.position;
    }
}
