using UnityEngine;

namespace ThroughTheWoods
{
    public class ArrowArcher : MonoBehaviour
    {
        int damage;
        SPUM_Enemy root;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        public void SetRootPlayer(SPUM_Enemy rootSet)
        {
            root = rootSet;
        }
        public void SetDamage(int damage)
        {
            this.damage = damage;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Health health = collision.GetComponent<Health>();
                if (!health.IsDead())
                {
                    health.TakeDamage(damage, false);

                    Destroy(gameObject);
                }
            }
            else if (collision.CompareTag("WallAndSprite"))
            {
                Destroy(gameObject);
            }
        }
    }
}