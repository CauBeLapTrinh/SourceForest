using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Forest
{
    public class EnemyHealth : MonoBehaviour
    {
        public float maxHealth;
        float currentHealth;

        public ProgressBar healthBar;

        public float damage = 5;

        float damageRate = 1f;
        float nextDame = 0;

        // Start is called before the first frame update
        void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxValue(maxHealth);
            healthBar.SetValue(maxHealth);

            healthBar.gameObject.SetActive(false);
        }

        // Update is called once per frame

        public void AddDamage(float damage)
        {
            currentHealth -= damage;

            healthBar.gameObject.SetActive(true);

            healthBar.SetValue(currentHealth);
            if (currentHealth <= 0)
            {
                MakeDead();
            }
        }

        void MakeDead()
        {
            if (gameObject.name == "Enemy1")
            {
                ControlPanel.instance.countBeerEnemy++;
                ControlPanel.instance.countBeerKill.text = ControlPanel.instance.countBeerEnemy.ToString() + " / " + ControlScenes.instance.amountBeer;
            }
            else if (gameObject.name == "Enemy2")
            {
                ControlPanel.instance.countTreeEnemy++;
                ControlPanel.instance.countTreeKill.text = ControlPanel.instance.countTreeEnemy.ToString() + " / " + ControlScenes.instance.amountTree;
            }

            Instantiate(Controller.instance.enemyDeadAnim, transform.position, Quaternion.identity);
            Instantiate(Controller.instance.itemDrop[IndexItemDrop()], transform.position, transform.rotation);
            gameObject.SetActive(false);

            ControlScenes.instance.CheckWin();
        }



        int IndexItemDrop()
        {
            int random = Random.Range(0, 146);
            if (random <= 10) return 0;
            else if (random <= 25) return 1;
            else if (random <= 55) return 2;
            else if (random <= 95) return 3;
            else return 4;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (Time.time > nextDame)
            {
                if (collision.gameObject.name == "Player")
                {
                    PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
                    player.addDamage(damage);
                }

                nextDame = Time.time + damageRate;
            }
        }
    }

}
