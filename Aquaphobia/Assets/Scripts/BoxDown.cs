using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDown : MonoBehaviour {

    public Transform end;
    float slerpTime;
    float slerpAcceleration;
    bool slerp = false;

    private void Update()
    {
        if (slerp)
        {
            transform.position = Vector3.Slerp(transform.position, end.position, slerpTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, end.rotation, slerpTime);
        }

        if (slerpTime >= 1 && slerp)
        {
            slerp = false;
            transform.position = end.position;
            transform.rotation = end.rotation;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            slerp = true;
            slerpAcceleration += Time.fixedDeltaTime;
            slerpTime += slerpAcceleration;
        }
    }
}
