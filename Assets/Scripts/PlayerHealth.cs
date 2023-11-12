using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static float maxHealth;
    float currentHealth;

    public HealthBar healthBar;
    public GameObject deadAnim;
    public Text healthText;
    public AudioSource hurtSound;

    static int getSceneIndex;
    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();

        getSceneIndex = scene.buildIndex;

        maxHealth = Player.health;
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
        healthBar.setHealth(maxHealth);

        healthText.text = currentHealth + "/" + maxHealth;
    }
    public void addHealth(float amount)
    {
        currentHealth += amount;

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.setHealth(currentHealth);

        healthText.text = currentHealth + "/" + maxHealth;
    }

    public void addDamage(float damage)
    {
        currentHealth -= damage;

        healthBar.setHealth(currentHealth);
        healthText.text = currentHealth + "/" + maxHealth;
        hurtSound.Play();

        if (currentHealth <= 0)
        {
            Instantiate(deadAnim, transform.position, Quaternion.identity);
            gameObject.SetActive(false);

            Invoke("reBorn", 2f);
        }
    }
    public void reBorn()
    {
        SceneManager.LoadScene(getSceneIndex);
    }
}
