using System.Collections;
using UnityEngine;

namespace ThroughTheWoods
{
    public class SPUM_DevilBoss : SPUM_Enemy
    {
        public Transform posAttack;
        public GameObject fireballPrefab;
        float delayAttack = 0;
        float delayFindPlayer = 1;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public override void Start()
        {
            base.Start();
            SetStateAnimationIndex(PlayerState.ATTACK, 4);
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            if (delayAttack >= 0)
            {
                delayAttack -= Time.deltaTime;
            }

            if (isAttack)
            {
                if (delayAttack < 0)
                {
                    Attack();
                    delayAttack = 2.5f;
                }
            }

            if (delayFindPlayer >= 0)
            {
                delayFindPlayer -= Time.deltaTime;

                if (delayFindPlayer < 0)
                {
                    FindPlayer();
                    delayFindPlayer = 1f;
                }
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        public void FindPlayer()
        {
            if (targetFollow != null)
                return;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, rangeAttack, Controller.instance.playerLayer);
            if (colliders.Length > 0)
            {
                foreach (Collider2D collider in colliders)
                {
                    if (collider.TryGetComponent(out Health health))
                    {
                        if (!health.IsDead())
                        {
                            targetFollow = collider.transform;
                        }
                    }
                }
            }
        }
        public void Attack()
        {
            if (targetFollow == null)
                return;

            if (targetFollow.TryGetComponent(out Health health))
            {
                if (health.IsDead())
                {
                    targetFollow = null;
                    return;
                }
            }

            if (!IsDead())
                StartCoroutine(Attack1(damageDefault, targetFollow.position));
        }
        public IEnumerator Attack1(int damage, Vector3 target)
        {
            PlayStateAnimation(PlayerState.ATTACK);
            yield return new WaitForSeconds(0.3f);
            CreateFireBall(damage, target);
            yield return new WaitForSeconds(0.2f);
            isAction = false;
        }
        public void CreateFireBall(int damage, Vector3 target)
        {
            Vector2 offset = new(0, 0.5f);
            target += (Vector3)offset;
            Vector2 direction = (target - posAttack.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            GameObject fireball = Instantiate(fireballPrefab, posAttack.position, Quaternion.Euler(0, 0, angle));
            Physics2D.IgnoreCollision(fireball.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            FireballWitch fireballScript = fireball.GetComponent<FireballWitch>();
            fireballScript.targetFollow = targetFollow;
            int dame = Random.Range(damage - 3, damage + 3);
            fireballScript.SetDamage(dame);
            fireballScript.SetRootPlayer(this);
            // Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            // rb.AddForce(direction * 15f, ForceMode2D.Impulse);
        }
    }
}

