using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack instance;

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

    private void Awake()
    {
        instance = this;
    }
    // Update is called once per frame
    private void Start()
    {
        damage = Player.damage;
        textDamage.text = "Attack: " + damage.ToString();

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !ControlMiniMap.isMiniMapOn && !ControlPanel.instance.IsOnPanelMission())
        {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            FireBullet();
        }
    }


    void FireBullet()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            if (mousePos.x >= transform.position.x)
            {
                spriteRenderer.flipX = false;
                animator.SetTrigger("Attack");

                StartCoroutine(ShootRight());
            }
            else
            {
                spriteRenderer.flipX = true;
                animator.SetTrigger("Attack");
                StartCoroutine(ShootLeft());
            }
        }
    }

    IEnumerator ShootLeft()
    {
        yield return new WaitForSeconds(0.3f);

        GameObject arrow = Instantiate(arrowPrefab, parentRotation.position, parentRotation.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(parentRotation.up * bulletForce, ForceMode2D.Impulse);

        audioSource.Play();
    }
    IEnumerator ShootRight()
    {
        yield return new WaitForSeconds(0.3f);

        GameObject arrow = Instantiate(arrowPrefab, parentRotation.position, parentRotation.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(parentRotation.up * bulletForce, ForceMode2D.Impulse);
        audioSource.Play();
    }
}
