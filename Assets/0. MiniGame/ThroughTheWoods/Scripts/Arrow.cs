using UnityEngine;

namespace ThroughTheWoods
{
    public class Arrow : MonoBehaviour
    {
        int damage;
        bool isCristical;
        SPUM_PlayerController rootPlayer;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        public void SetRootPlayer(SPUM_PlayerController player)
        {
            rootPlayer = player;
        }
        public void SetDamage(int damage, bool isCristical)
        {
            this.damage = damage;
            this.isCristical = isCristical;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                IEnemyTarget enemyI = collision.GetComponent<IEnemyTarget>();
                enemyI.SetTargetFollow(rootPlayer.transform);
                Health health = collision.GetComponent<Health>();
                if (!health.IsDead())
                {
                    health.TakeDamage(damage, isCristical);
                    rootPlayer.CalculateDamage(damage);

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