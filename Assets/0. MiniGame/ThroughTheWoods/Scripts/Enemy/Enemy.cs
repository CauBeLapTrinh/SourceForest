using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace ThroughTheWoods
{
    public class Enemy : MonoBehaviour
    {
        [Header("---- Properties ----")]
        public float maxHeal;
        float currentHeal;
        Animator animator;
        bool isDead = false;
        [Header("Attack")]
        public float damage;
        float delayAttack = 0;
        [Header("AI Movement")]
        public float speed;
        public float movementRange = 3;
        float idleTime = 2f;
        bool isFacingRight = true;
        Vector2 limitRangeX;
        Vector2 limitRangeY;
        Vector3 targetPosition;
        Transform targetFollow = null;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            animator = GetComponent<Animator>();
            currentHeal = maxHeal;
            limitRangeX = new Vector2(transform.position.x - movementRange, transform.position.x + movementRange);
            limitRangeY = new Vector2(transform.position.y - movementRange, transform.position.y + movementRange);
            RandomTagetPos();
        }

        // Update is called once per frame
        void Update()
        {
            //animator.SetFloat("Movement", 0);
        }
        void FixedUpdate()
        {
            if (delayAttack >= 0)
            {
                delayAttack -= Time.fixedDeltaTime;
            }

            if (!isDead)
            {
                Movement();
            }
        }
        public void Movement()
        {
            if (targetFollow != null)
            {
                float distanceTarget = Vector2.Distance(transform.position, targetFollow.position);

                if (distanceTarget < 3f && RangeCheck())
                {
                    FollowTarget(distanceTarget);
                }
                else
                {
                    targetFollow = null;
                    idleTime = 0;
                    RandomTagetPos();
                }
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
        public void FollowTarget(float distance)
        {
            FaceCheck(targetFollow.position);
            if (distance > 0.5f)
            {
                animator.SetFloat("Movement", 1);
                transform.position = Vector3.MoveTowards(transform.position, targetFollow.position, speed * Time.fixedDeltaTime);
            }
            else
            {
                animator.SetFloat("Movement", 0);
                AutoAttack();
            }
        }
        public void AutoMovement()
        {
            FaceCheck(targetPosition);
            animator.SetFloat("Movement", 1);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);

            if (transform.position == targetPosition)
            {
                idleTime = Random.Range(3f, 4f);
                animator.SetFloat("Movement", 0);
                RandomTagetPos();
            }
        }
        public void AutoAttack()
        {
            if (delayAttack < 0)
            {
                animator.SetTrigger("Attack");
                delayAttack = 2f;
            }
        }
        public void Attack()
        {
            PlayerController playerScript = targetFollow.GetComponent<PlayerController>();
            playerScript.Hit(damage);
        }
        public void FaceCheck(Vector2 target)
        {
            if (target.x < transform.position.x && isFacingRight)
            {
                FaceSet(-1);
            }
            else if (target.x > transform.position.x && !isFacingRight)
            {
                FaceSet(1);
            }
        }
        public bool RangeCheck()
        {
            if (transform.position.x > limitRangeX.x && transform.position.x < limitRangeX.y)
            {
                if (transform.position.y > limitRangeY.x && transform.position.y < limitRangeY.y)
                {
                    return true;
                }
            }
            return false;
        }
        public void RandomTagetPos()
        {
            float rdX = Random.Range(limitRangeX.x, limitRangeX.y);
            float rdY = Random.Range(limitRangeY.x, limitRangeY.y);
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
        public void SetTargetFollow(Transform targetSet)
        {
            targetFollow = targetSet;
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
        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(new Vector3(limitRangeX.x, limitRangeY.y), new Vector3(limitRangeX.y, limitRangeY.y));
            Gizmos.DrawLine(new Vector3(limitRangeX.x, limitRangeY.x), new Vector3(limitRangeX.y, limitRangeY.x));
            Gizmos.DrawLine(new Vector3(limitRangeX.y, limitRangeY.x), new Vector3(limitRangeX.y, limitRangeY.y));
            Gizmos.DrawLine(new Vector3(limitRangeX.x, limitRangeY.x), new Vector3(limitRangeX.x, limitRangeY.y));
        }
    }
}

