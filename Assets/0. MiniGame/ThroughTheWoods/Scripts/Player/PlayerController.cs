using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheWoods
{
    public class PlayerController : MonoBehaviour
    {
        Animator animator;
        PlayerMovement playerMovement;
        bool isAttacking = false;
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        // Update is called once per frame
        void Update()
        {
            // Kiểm tra nếu người chơi click chuột
            if (Input.GetMouseButtonDown(0) && !isAttacking) // 0 là nút chuột trái
            {
                // Lấy vị trí click chuột trong thế giới game
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0; // Đảm bảo z = 0 để không ảnh hưởng đến 2D

                // Tính vector hướng từ nhân vật đến vị trí click chuột
                Vector2 direction = (mousePosition - transform.position).normalized;
                // Xác định hướng bắn
                DetermineShootDirection(direction);
            }
        }

        void DetermineShootDirection(Vector2 direction)
        {
            playerMovement.SetCanMove(false);
            playerMovement.StopMovement();
            // Xác định hướng dựa trên vector hướng
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Hướng ngang (trái/phải)
                if (direction.x > 0)
                {
                    playerMovement.SetLookDirection(LookDirection.Right);
                }
                else
                {
                    playerMovement.SetLookDirection(LookDirection.Left);
                }
            }
            else
            {
                // Hướng dọc (lên/xuống)
                if (direction.y > 0)
                {
                    playerMovement.SetLookDirection(LookDirection.Back);
                }
                else
                {
                    playerMovement.SetLookDirection(LookDirection.Front);
                }
            }

            Attack();
        }
        public void Attack()
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
        }
        public void AttackEnd()
        {
            isAttacking = false;
            playerMovement.SetCanMove(true);
        }
    }
}

