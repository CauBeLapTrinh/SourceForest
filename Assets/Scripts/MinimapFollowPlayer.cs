using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFollowPlayer : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }
}
