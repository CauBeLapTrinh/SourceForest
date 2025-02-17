using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheWoods
{
    public enum LookDirection
    {
        Back,
        Front,
        Left,
        Right
    }
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Properties")]
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
            rb.linearVelocity = runSpeed * movement;
        }
        public void StopMovement()
        {
            rb.velocity = Vector2.zero;
        }
        public void SetCanMove(bool move)
        {
            canMove = move;
        }
        public void SetLastMove(int value)
        {
            lastMove = value;
        }
        public void SetLookDirection(LookDirection direction)
        {
            switch (direction)
            {
                case LookDirection.Back:
                    animator.SetFloat("Horizontal", 0);
                    animator.SetFloat("Vertical", 1);
                    lastMove = 2;
                    break;
                case LookDirection.Front:
                    animator.SetFloat("Horizontal", 0);
                    animator.SetFloat("Vertical", -1);
                    lastMove = 0;
                    break;
                case LookDirection.Left:
                    animator.SetFloat("Horizontal", -1);
                    animator.SetFloat("Vertical", 0);

                    if (isFacingRight)
                    {
                        FlipFace();
                    }

                    lastMove = 1;
                    break;
                case LookDirection.Right:
                    animator.SetFloat("Horizontal", 1);
                    animator.SetFloat("Vertical", 0);

                    if (!isFacingRight)
                    {
                        FlipFace();
                    }

                    lastMove = 1;
                    break;
                default:
                    break;
            }
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

