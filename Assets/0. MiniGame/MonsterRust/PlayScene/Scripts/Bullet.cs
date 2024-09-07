using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.MonsterRush
{
    public class Bullet : MonoBehaviour
    {
        float damage;
        
        bool isThrough = false;
        Rigidbody2D rb;
        float speedBullet;
        Vector2 direc;
        bool isHit = false;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetDamage(float setDamage)
        {
            damage = setDamage;
        }
        public void Shoot(Vector2 direction, float speed)
        {
            rb = GetComponent<Rigidbody2D>();

            direc = direction;
            speedBullet = speed;
            rb.velocity = direc * speedBullet;
        }

        public void SetThrough()
        {
            isThrough = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy") && !isHit)
            {
                isHit = true;

                MonsterControl monsterControl = collision.GetComponent<MonsterControl>();
                monsterControl.TakeDame(damage);

                if (!isThrough)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
