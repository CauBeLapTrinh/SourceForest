using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.MonsterRush
{
    public class PlayerControl : MonoBehaviour
    {

        [Header("--------- Properties ---------")]
        [Header("--- Attack ---")]
        public float damage;
        public float rangeAttack;
        public float speedAttack;
        public float speedBullet;

        float nextAttack = 0;
        [Header("--- Health ---")]
        public float maxHealth;
        float currentHealth;

        [Header("--------- BulletPrefab ---------")]
        public GameObject bulletPrefabs;

        [Header("--------- HealthBar ---------")]
        public ProgressBar healthBar;

        bool isDead = false;
        // Start is called before the first frame update
        void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxValue(maxHealth);
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time > nextAttack)
            {
                Attack();
                nextAttack = Time.time + 1 / speedAttack;
            }
        }

        public Transform GetEnemyNearest()
        {
            Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, rangeAttack, Controller.instance.enemyLayer);

            if (colls.Length == 0)
            {
                return null;
            }

            float minDistance = 100f;
            int indexNearest = 0;
            for (int i = 0; i < colls.Length; i++)
            {
                float distanceCurrent = Vector2.Distance(colls[i].transform.position, transform.position);

                if (distanceCurrent < minDistance)
                {
                    minDistance = distanceCurrent;
                    indexNearest = i;
                }
            }
            return colls[indexNearest].transform;
        }
        public void Attack()
        {
            Transform enemyNearest = GetEnemyNearest();
            if (enemyNearest != null)
            {
                Vector3 eulerBullet = enemyNearest.transform.position - transform.position;

                CreateBullet(eulerBullet);
            }
        }

        public void CreateBullet(Vector3 eulerAngle)
        {
            float zAxis = Mathf.Atan2(eulerAngle.x, eulerAngle.y) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.Euler(0, 0, -zAxis);

            GameObject bullet = Instantiate(bulletPrefabs, transform.position, rotation);

            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>());
            Bullet sciptBullet = bullet.GetComponent<Bullet>();
            sciptBullet.SetDamage(damage);

            float angle = (bullet.transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad;

            Vector2 vectorF = new(Mathf.Cos(angle), Mathf.Sin(angle));
            sciptBullet.Shoot(vectorF, speedBullet);
        }
        public void TakeDame(float damage)
        {
            if (isDead) return;

            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                isDead = true;

                currentHealth = 0;
            }

            healthBar.SetValue(currentHealth);
            healthBar.SetText($"{currentHealth}/{maxHealth}");
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, rangeAttack);
        }
    }
}
