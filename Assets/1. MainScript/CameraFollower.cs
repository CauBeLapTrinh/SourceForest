using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [Header("Camera Follow")]
    public Transform targetFollow;
    public Vector3 offSet;
    public float smoot;
    Vector3 vel = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        if (targetFollow == null)
        {
            targetFollow = GameObject.Find("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tagetPosition = new Vector3(targetFollow.position.x, targetFollow.position.y, 0) + offSet;

        transform.position = Vector3.SmoothDamp(transform.position, tagetPosition, ref vel, smoot);
    }
}
