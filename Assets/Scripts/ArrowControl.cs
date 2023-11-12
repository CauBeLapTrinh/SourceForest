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

    public void removeForce()
    {
        myBody.velocity = new Vector2(0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            enemy.addDamage(damage);
            removeForce();
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "WallAndSprite")
        {
            removeForce();
            Destroy(gameObject);
        }
    }
}
