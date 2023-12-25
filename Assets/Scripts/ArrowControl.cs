using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControl : MonoBehaviour
{
    float damage = PlayerAttack.damage;

    Rigidbody2D myBody;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    public void RemoveForce()
    {
        myBody.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            enemy.AddDamage(damage);
            RemoveForce();
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "WallAndSprite")
        {
            RemoveForce();
            Destroy(gameObject);
        }
    }
}
