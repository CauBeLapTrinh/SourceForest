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
    [Header("Limited Setup")]
    public bool isLimited = false;
    public Vector2 minLimit;
    public Vector2 maxLimit;
    // Start is called before the first frame update
    void Start()
    {
        if (targetFollow == null)
        {
            targetFollow = GameObject.Find("Player").transform;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 tagetPosition = new Vector3(targetFollow.position.x, targetFollow.position.y, 0) + offSet;

        if (isLimited)
        {
            if (tagetPosition.x < minLimit.x)
            {
                tagetPosition.x = minLimit.x;
            }
            else if (tagetPosition.x > maxLimit.x)
            {
                tagetPosition.x = maxLimit.x;
            }

            if (tagetPosition.y < minLimit.y)
            {
                tagetPosition.y = minLimit.y;
            }
            else if (tagetPosition.y > maxLimit.y)
            {
                tagetPosition.y = maxLimit.y;
            }
        }

        transform.position = Vector3.SmoothDamp(transform.position, tagetPosition, ref vel, smoot);
    }
}
