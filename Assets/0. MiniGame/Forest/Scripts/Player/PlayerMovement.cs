using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Minigame.Forest
{

    public class PlayerMovement : MonoBehaviour
    {
        public float maxSpeed;
        Rigidbody2D rb;
        Vector2 movement;
        bool canMove;
        Animator animator;
        SpriteRenderer spriteRenderer;
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
            else if (!canMove)
            {
                movement = Vector2.zero;
            }
        }
        private void FixedUpdate()
        {
            if (canMove && !ControlMiniMap.isMiniMapOn)
            {
                rb.MovePosition(rb.position + movement * maxSpeed * Time.fixedDeltaTime);
            }
        }
        public IEnumerator StopToFire()
        {
            canMove = false;

            yield return new WaitForSeconds(0.3f);

            canMove = true;
        }
    }
}
