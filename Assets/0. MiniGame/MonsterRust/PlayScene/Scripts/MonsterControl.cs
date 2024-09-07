using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.MonsterRush
{
    public class MonsterControl : MonoBehaviour
    {
        public Transform targetTransform;

        [Header("--------- Properties ---------")]
        public int levelMonster;
        [Header("--- Movement ---")]
        public float speed;
        [Header("--- Attack ---")]
        public float damage;
        public float speedAttack;
        float nextAttack = 0;
        [Header("--- Health ---")]
        public float maxHealth;
        float currentHealth;

        bool isDead = false;
        Animator animator;
        SpriteRenderer spriteRenderer;
        // Start is called before the first frame update
        void Start()
        {
            targetTransform = Controller.instance.player;

            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            currentHealth = maxHealth;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time > nextAttack)
            {
                if (Vector2.Distance(transform.position, targetTransform.position) < 0.5f)
                {
                    Attack();

                    nextAttack = Time.time + 1 / speedAttack;
                }
            }
        }
        private void FixedUpdate()
        {
            if (!isDead)
            {
                FollowPlayer();
            }
        }

        void FollowPlayer()
        {
            if (transform.position.x - targetTransform.transform.position.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetTransform.transform.position, speed * Time.fixedDeltaTime);
            animator.SetFloat("Walk", 1);
        }
        public void SetTarget(Transform setTarget)
        {
            targetTransform = setTarget;
        }
        public void Attack()
        {
            Controller.instance.playerScript.TakeDame(damage);
        }
        public void TakeDame(float damage)
        {
            currentHealth -= damage;

            GameObject textHit = Instantiate(Controller.instance.textHit, transform.position, Quaternion.identity);
            
            TextHit scriptText = textHit.GetComponent<TextHit>();
            scriptText.SetText($"{damage}");

            if (currentHealth <= 0)
            {
                Dead();
            }
        }
        public void Dead()
        {
            isDead = true;

            Instantiate(Controller.instance.enemyDeadAnim, transform.position, Quaternion.identity);
            Instantiate(Controller.instance.exps[levelMonster], transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
