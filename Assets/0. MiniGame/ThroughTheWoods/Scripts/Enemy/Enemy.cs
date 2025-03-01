using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace ThroughTheWoods
{
    public class Enemy : MonoBehaviour
    {
        [Header("Properties")]
        public float maxHeal;
        float currentHeal;
        Animator animator;
        bool isDead = false;
        [Header("AI Movement")]
        public float speed;
        public float movementRadius = 3;
        float idleTime = 2f;
        bool isFacingRight = true;
        Vector3 targetPosition;
        Transform targetFollow = null;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            animator = GetComponent<Animator>();
            currentHeal = maxHeal;
        }

        // Update is called once per frame
        void Update()
        {
            //animator.SetFloat("Movement", 0);
        }
        void FixedUpdate()
        {
            if (!isDead)
            {
                if (targetFollow != null)
                {
                    FollowTarget();
                }
                else
                {
                    idleTime -= Time.fixedDeltaTime;
                    if (idleTime <= 0)
                    {
                        AutoMovement();
                    }
                }
            }
        }
        public void FollowTarget()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetFollow.position, speed * Time.fixedDeltaTime);
        }
        public void AutoMovement()
        {
            if (targetPosition.x < transform.position.x && isFacingRight)
            {
                FaceSet(-1);
            }
            else if (targetPosition.x > transform.position.x && !isFacingRight)
            {
                FaceSet(1);
            }
            animator.SetFloat("Movement", 1);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);

            if (transform.position == targetPosition)
            {
                idleTime = Random.Range(3f, 4f);
                animator.SetFloat("Movement", 0);
                RandomTagetPos();
            }
        }
        public void RandomTagetPos()
        {
            float rdX = Random.Range(-movementRadius, movementRadius);
            float rdY = Random.Range(-movementRadius, movementRadius);
            targetPosition = new Vector2(rdX, rdY);
        }
        public void FaceSet(float dir)
        {
            if (dir == -1)
            {
                isFacingRight = false;
            }
            else
            {
                isFacingRight = true;
            }
            Vector3 theScale = transform.localScale;
            theScale.x = dir;
            transform.localScale = theScale;
        }
        public void Hit(float damage)
        {
            currentHeal -= damage;
            animator.SetTrigger("Hit");

            Vector2 posSpawn = transform.position + Vector3.up;
            GameObject textHit = Instantiate(Controller.instance.textHit, posSpawn, Quaternion.identity);

            TextHit scriptText = textHit.GetComponent<TextHit>();
            scriptText.SetText($"{damage}");

            if (currentHeal <= 0)
            {
                Dead();
            }
        }
        public void Dead()
        {
            isDead = true;
            animator.SetTrigger("Dead");
        }
    }
}

