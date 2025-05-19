using UnityEngine;

namespace ThroughTheWoods
{
    public class FireballWitch : MonoBehaviour
    {
        int damage;
        SPUM_Enemy root;
        public GameObject explosionPrefab;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void OnDestroy()
        {

            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1, Controller.instance.playerLayer);
            foreach (Collider2D collider in colliders)
            {
                Health health = collider.GetComponent<Health>();
                if (!health.IsDead())
                {
                    health.TakeDamage(damage, false);
                }
            }
        }
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
            if (collision.CompareTag("Player") || collision.CompareTag("WallAndSprite"))
            {
                Destroy(gameObject);
            }
        }
    }
}
