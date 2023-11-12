using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoDestroy : MonoBehaviour
{
    public float autoDestroyTime;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyMe", autoDestroyTime);
    }
    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
