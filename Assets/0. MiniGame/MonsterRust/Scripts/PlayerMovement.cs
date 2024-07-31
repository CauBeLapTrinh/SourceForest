using Minigame.Forest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.MonsterRush
{
    public class PlayerMovement : MonoBehaviour
    {
        public float maxSpeed;

        Rigidbody2D rb;
        Vector2 movement;
        bool canMove = true;

        Animator animator;
        PlayerControl playerControl;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();
            playerControl = GetComponentInChildren<PlayerControl>();
        }

        // Update is called once per frame
        void Update()
        {
            if (canMove)
            {
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");

                //animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Speed", movement.sqrMagnitude);

                if (!playerControl.IsNearEnemy())
                {
                    if (movement.x < 0 && playerControl.IsFacingRight())
                    {
                        playerControl.SwapFace();
                    }
                    else if (movement.x > 0 && !playerControl.IsFacingRight())
                    {
                        playerControl.SwapFace();
                    }
                }
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
                rb.MovePosition(rb.position + movement * maxSpeed * Time.fixedDeltaTime);
            }
        }
    }
}
