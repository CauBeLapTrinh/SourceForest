using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Minigame.Forest
{

public class PlayerHealth : MonoBehaviour
{
    public static float maxHealth;
    float currentHealth;

    public ProgressBar healthBar;
    public GameObject deadAnim;
    public Text healthText;
    public AudioSource hurtSound;

    static int getSceneIndex;
    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();

        getSceneIndex = scene.buildIndex;

        maxHealth = StaticPropertis.health;
        currentHealth = maxHealth;
        healthBar.SetMaxValue(maxHealth);
        healthBar.SetValue(maxHealth);

        healthText.text = currentHealth + "/" + maxHealth;
    }
    public void addHealth(float amount)
    {
        currentHealth += amount;

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.SetValue(currentHealth);

        healthText.text = currentHealth + "/" + maxHealth;
    }

    public void addDamage(float damage)
    {
        currentHealth -= damage;

        healthBar.SetValue(currentHealth);
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
}
