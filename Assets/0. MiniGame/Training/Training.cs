using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Training : MonoBehaviour
{
    public Transform tagetHook;

    Rigidbody2D rb;
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, tagetHook.position);
    }
}
