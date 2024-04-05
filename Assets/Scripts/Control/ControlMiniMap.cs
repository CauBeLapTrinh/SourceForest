using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMiniMap : MonoBehaviour
{
    public static ControlMiniMap instance;
        
    public static bool isMiniMapOn;
    public float speed;

    [Header("Limit position")]
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;

    Rigidbody2D rb;
    Vector2 movement;

    private void Awake()
    {
        instance = this;
        
        isMiniMapOn = false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isMiniMapOn)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (transform.position.x <= maxX
                && transform.position.x >= minX
                && transform.position.y <= maxY
                && transform.position.y >= minY)
            {
                rb.velocity = new Vector2(movement.x * speed, movement.y * speed);
            }
            else rb.velocity = Vector2.zero;

            //rb.velocity = new Vector2(movement.x * speed, movement.y * speed);

        }
    }
}
