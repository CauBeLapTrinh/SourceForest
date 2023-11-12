using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public static float damage;
    public Text textDamage;
    public GameObject arrowPrefab;
    public AudioSource audioSource;
    float bulletForce = 20f;

    Animator animator;
    SpriteRenderer spriteRenderer;

    float fireRate = 0.3f;
    float nextFire = 0;

    public Camera cam;
    Vector2 mousePos;

    public Transform parentRotation;

    // Update is called once per frame
    private void Start()
    {
        textDamage.text = damage.ToString();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        damage = Player.damage;
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            fireBullet();
        }

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        textDamage.text = damage.ToString();
    }


    void fireBullet()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            if (mousePos.x >= transform.position.x)
            {
                spriteRenderer.flipX = false;
                animator.SetTrigger("Attack");
                Invoke("ShootRight", 0.3f);
            }
            else
            {
                spriteRenderer.flipX = true;
                animator.SetTrigger("Attack");
                Invoke("ShootLeft", 0.3f);
            }
        }
    }

    void ShootLeft()
    {
        GameObject arrow = Instantiate(arrowPrefab, parentRotation.position, parentRotation.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(parentRotation.up * bulletForce, ForceMode2D.Impulse);

        audioSource.Play();
    }
    void ShootRight()
    {
        GameObject arrow = Instantiate(arrowPrefab, parentRotation.position, parentRotation.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(parentRotation.up * bulletForce, ForceMode2D.Impulse);
        audioSource.Play();
    }
}
