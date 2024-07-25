using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;

    Rigidbody2D rb;
    float speedBullet;
    Vector2 direc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(Vector2 direction, float speed)
    {
        rb = GetComponent<Rigidbody2D>();

        direc = direction;
        speedBullet = speed;
        rb.velocity = direc * speedBullet;
    }
}
