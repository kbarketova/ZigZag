using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{

    public void Set(Transform target, Vector3 offset, float smoothing)
    {
        var currentVelocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref currentVelocity, smoothing);
    }
}
