using UnityEngine;
using System.Collections;

namespace ThroughTheWoods
{
    public class SPUM_SkeletonArcher : SPUM_Enemy
    {
        public Transform posAttack;
        public GameObject arrowPrefab;
        float delayAttack = 0;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public override void Start()
        {
            base.Start();
            SetStateAnimationIndex(PlayerState.ATTACK, 3);
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
                    delayAttack = 2f;
                }
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        public void Attack()
        {
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
            CreateArrow(damage, target);
            yield return new WaitForSeconds(0.2f);
            isAction = false;
        }
        public void CreateArrow(int damage, Vector3 target)
        {
            Vector2 offset = new(0, 0.5f);
            target += (Vector3)offset;
            Vector2 direction = (target - posAttack.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            GameObject arrow = Instantiate(arrowPrefab, posAttack.position, Quaternion.Euler(0, 0, angle));
            Physics2D.IgnoreCollision(arrow.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            ArrowArcher arrowScript = arrow.GetComponent<ArrowArcher>();
            int dame = Random.Range(damage - 3, damage + 3);
            arrowScript.SetDamage(dame);
            arrowScript.SetRootPlayer(this);
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * 12f, ForceMode2D.Impulse);
        }
    }
}
