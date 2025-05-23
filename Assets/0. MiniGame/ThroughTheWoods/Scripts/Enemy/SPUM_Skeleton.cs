using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ThroughTheWoods
{
    public class SPUM_Skeleton : SPUM_Enemy
    {
        float delayAttack = 0;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public override void Start()
        {
            base.Start();
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
                    delayAttack = 1 / speedAttack;
                }
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        public void Attack()
        {
            if (!IsDead())
                StartCoroutine(Attack1());
        }
        public IEnumerator Attack1()
        {
            isAction = true;
            PlayStateAnimation(PlayerState.ATTACK);
            yield return new WaitForSeconds(0.2f);

            if (!targetFollow.TryGetComponent<Health>(out var health))
            {
                isAction = false;
                yield break;
            }

            int dame = Random.Range(damageDefault - 3, damageDefault + 3);
            health.TakeDamage(dame, false);

            yield return new WaitForSeconds(0.2f);
            isAction = false;
        }
    }
}