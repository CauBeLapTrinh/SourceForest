using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheWoods
{
    public class PlayerMovement : MonoBehaviour
    {
        public float runSpeed;
        Rigidbody2D rb;
        Vector2 movement;
        bool canMove = true;
        Animator animator;
        bool isFacingRight = true;
        int lastMove = 0;
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                ButtonDownMoveBack();
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ButtonDownMoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                ButtonDownMoveFront();
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                ButtonDownMoveRight();
            }

            if (canMove)
            {
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");

                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
                animator.SetFloat("Movement", movement.sqrMagnitude);
                animator.SetFloat("Idle", lastMove);

                FaceControl();
            }
            else if (!canMove)
            {
                movement = Vector2.zero;
            }
        }
        private void FixedUpdate()
        {
            if (canMove)
            {
                Movement();
            }
        }
        public void FaceControl()
        {
            if (movement.x < 0 && isFacingRight)
            {
                FlipFace();
            }
            else if (movement.x > 0 && !isFacingRight)
            {
                FlipFace();
            }
        }
        public void FlipFace()
        {
            isFacingRight = !isFacingRight;
            Vector2 theScale = transform.localScale;
            theScale.x = -theScale.x;
            transform.localScale = theScale;
        }
        public void Movement()
        {
            rb.velocity = runSpeed * movement;
        }
        public IEnumerator StopToFire()
        {
            canMove = false;

            yield return new WaitForSeconds(0.3f);

            canMove = true;
        }
        public void ButtonDownMoveBack()
        {
            lastMove = 2;
        }
        public void ButtonDownMoveFront()
        {
            lastMove = 0;
        }
        public void ButtonDownMoveLeft()
        {
            lastMove = 1;
        }
        public void ButtonDownMoveRight()
        {
            lastMove = 1;
        }

    }
}

