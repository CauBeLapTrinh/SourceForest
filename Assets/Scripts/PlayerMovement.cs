using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxSpeed;

    Rigidbody2D rb;
    Vector2 movement;
    bool canMove;

    Animator animator;

    SpriteRenderer spriteRenderer;

    public Camera cam;
    Vector2 mousePos;
    public Transform rotationFirePoint;
    public Transform FirePointLeft;
    public Transform FirePointRight;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !ControlMiniMap.isMiniMapOn)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);

            if (movement.x < 0 && !spriteRenderer.flipX)
            {
                spriteRenderer.flipX = true;
            }
            else if (movement.x > 0 && spriteRenderer.flipX)
            {
                spriteRenderer.flipX = false;
            }
        }
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetButtonDown("Fire1"))
        {
            canMove = false;
            Invoke("changeMove", 0.3f);
        }
    }
    private void FixedUpdate()
    {
        if (canMove && !ControlMiniMap.isMiniMapOn)
        {
            rb.MovePosition(rb.position + movement * maxSpeed * Time.fixedDeltaTime);

            Rigidbody2D rbFp = rotationFirePoint.GetComponent<Rigidbody2D>();
            Rigidbody2D rbFpLeft = FirePointLeft.GetComponent<Rigidbody2D>();
            Rigidbody2D rbFpRight = FirePointRight.GetComponent<Rigidbody2D>();

            if (mousePos.x >= transform.position.x)
            {
                rbFp.position = rbFpRight.position;
            }
            else if (mousePos.x < transform.position.x)
            {
                rbFp.position = rbFpLeft.position;
            }
            
            Vector2 lookDir = mousePos - rbFp.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rbFp.rotation = angle;
        }
    }
    void changeMove()
    {
        canMove = true;
    }
}
