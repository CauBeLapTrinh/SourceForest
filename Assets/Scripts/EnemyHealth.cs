using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public Text countBeerKill;
    public Text countTreeKill;

    public float maxHealth;
    float currentHealth;

    public HealthBar healthBar;

    public float damage = 5;

    public List<GameObject> itemDrop;

    public GameObject deadAnim;

    float damageRate = 1f;
    float nextDame = 0;

    RandomNumber randomNumber;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
        healthBar.setHealth(maxHealth);

        healthBar.gameObject.SetActive(false);

        randomNumber = new RandomNumber();
    }

    // Update is called once per frame

    public void addDamage(float damage)
    {
        currentHealth -= damage;

        healthBar.gameObject.SetActive(true);

        healthBar.setHealth(currentHealth);
        if (currentHealth <= 0)
        {
            if (gameObject.name == "Enemy1")
            {
                CountKillEnemy.countBeerEnemy++;
                countBeerKill.text = CountKillEnemy.countBeerEnemy.ToString();
            }
            else if (gameObject.name == "Enemy2")
            {
                CountKillEnemy.countTreeEnemy++;
                countTreeKill.text = CountKillEnemy.countTreeEnemy.ToString();
            }

            Instantiate(deadAnim, transform.position, Quaternion.identity);
            Instantiate(itemDrop[indexItemDrop()], transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }
    int indexItemDrop()
    {
        int random = randomNumber.rd.Next(1, 101);
        if (random <= 10) return 0;
        else if (random <= 30) return 1;
        else if(random <= 50) return 2;
        else return 3;
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
