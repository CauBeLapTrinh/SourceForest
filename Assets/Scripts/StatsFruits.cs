using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsFruits : MonoBehaviour
{
    public float plusHealth = 10;
    public float plusDamage = 5;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if(gameObject.name.IndexOf("Cherries") == 0)
            {
                PlayerAttack.damage += 5;
            }else if(gameObject.name.IndexOf("Orange") == 0)
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                playerHealth.addHealth(plusHealth);
            }
        }
    }
}
