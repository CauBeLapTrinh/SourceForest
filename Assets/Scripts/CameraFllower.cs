using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFllower : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;

    public Vector3 offset;

    private void FixedUpdate()
    {
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.fixedDeltaTime);

        transform.position = smoothPos;
    }
}
