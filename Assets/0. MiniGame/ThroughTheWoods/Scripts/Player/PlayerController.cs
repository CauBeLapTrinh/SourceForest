using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheWoods
{
    public class PlayerController : MonoBehaviour
    {
        Transform posAttack;
        public Transform posAttackFront, posAttackBack, posAttackSide;
        [Header("--- Properties ---")]
        public float damage;
        public ProgressBar healBar;
        public float maxHp;
        float currentHp;
        Animator animator;
        PlayerMovement playerMovement;
        bool isAttacking = false;
        int indexSkill = 0;
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            playerMovement = GetComponent<PlayerMovement>();

            currentHp = maxHp;
            healBar.SetMaxValue(maxHp);
            healBar.SetValue(currentHp);
            healBar.SetText($"{currentHp}/{maxHp}");
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
        public void Hit(float damage)
        {
            currentHp -= damage;

            Vector2 posSpawn = transform.position + Vector3.up;
            GameObject textHit = Instantiate(Controller.instance.controlPrefabs.textHit, posSpawn, Quaternion.identity);

            TextHit scriptText = textHit.GetComponent<TextHit>();
            scriptText.SetText($"-{damage}");

            if (currentHp <= 0)
            {
                currentHp = 0;

                Dead();
            }
            healBar.SetValue(currentHp);
            healBar.SetText($"{currentHp}/{maxHp}");
        }
        public void Dead()
        {

        }
        Collider2D[] enemys;
        public void HitEnemy()
        {
            if (enemys.Length > 0)
            {
                foreach (var enemy in enemys)
                {
                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    enemyScript.TakeDamage(damage, false);
                    enemyScript.SetTargetFollow(transform);
                }
            }
        }
        public void KinfeAttack()
        {
            enemys = Physics2D.OverlapBoxAll(posAttack.position, Vector2.one, 0, Controller.instance.enemyLayer);
        }
        public void WeaponAttack(Transform SetPosAttack)
        {
            posAttack = SetPosAttack;
            switch (indexSkill)
            {
                case 0:
                    KinfeAttack();
                    break;
                default:
                    break;
            }
        }
        public void Attack()
        {
            isAttacking = true;
            animator.SetTrigger("Attack");

            switch (playerMovement.GetLastMove())
            {
                case 0:
                    WeaponAttack(posAttackFront);
                    break;
                case 1:
                    WeaponAttack(posAttackSide);
                    break;
                case 2:
                    WeaponAttack(posAttackBack);
                    break;
                default:
                    break;
            }
        }
        public void AttackEnd()
        {
            isAttacking = false;
            playerMovement.SetCanMove(true);
        }
        void OnDrawGizmos()
        {
            if (isAttacking == false || posAttack == null)
                return;

            // Đặt màu cho Gizmos
            Gizmos.color = Color.red;

            // Vẽ hình hộp (box) bằng Gizmos
            Gizmos.DrawWireCube(posAttack.position, Vector3.one);
        }
    }
}

