using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Minigame.Forest
{

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack instance;

    [Header("Control")]
    public PlayerMovement playerMovement;

    [Header("Properties")]
    public static float damage;
    public Text textDamage;
    public GameObject arrowPrefab;
    public AudioSource audioSource;

    [Header("FirePoint")]
    public Transform rotationFirePoint;
    public Transform firePointLeft;
    public Transform firePointRight;
    float bulletForce = 20f;

    Animator animator;
    SpriteRenderer spriteRenderer;

    [Header("Action")]
    public float fireRate = 0.3f;
    float nextFire = 0;

    public Camera cam;
    Vector2 mousePos;
    private void Awake()
    {
        instance = this;
    }
    // Update is called once per frame
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        damage = StaticPropertis.damage;
        textDamage.text = "Attack: " + damage.ToString();

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !ControlMiniMap.isMiniMapOn && !ControlPanel.instance.IsOnPanelMission())
        {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            StartCoroutine(playerMovement.StopToFire());

            FireBullet();
        }
    }

    void FireBullet()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            Rigidbody2D rbFp = rotationFirePoint.GetComponent<Rigidbody2D>();

            if (mousePos.x >= transform.position.x)
            {
                rbFp.position = firePointRight.position;

                Vector2 lookDir = mousePos - rbFp.position;
                float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
                rbFp.rotation = angle;

                spriteRenderer.flipX = false;
                animator.SetTrigger("Attack");

                StartCoroutine(ShootRight());
            }
            else
            {
                rbFp.position = firePointLeft.position;

                Vector2 lookDir = mousePos - rbFp.position;
                float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
                rbFp.rotation = angle;

                spriteRenderer.flipX = true;
                animator.SetTrigger("Attack");

                StartCoroutine(ShootLeft());
            }
        }
    }

    IEnumerator ShootLeft()
    {
        yield return new WaitForSeconds(0.3f);

        GameObject arrow = Instantiate(arrowPrefab, firePointLeft.position, rotationFirePoint.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(rotationFirePoint.up * bulletForce, ForceMode2D.Impulse);

        audioSource.Play();
    }
    IEnumerator ShootRight()
    {
        yield return new WaitForSeconds(0.3f);

        GameObject arrow = Instantiate(arrowPrefab, firePointRight.position, rotationFirePoint.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(rotationFirePoint.up * bulletForce, ForceMode2D.Impulse);
        audioSource.Play();
    }
}
}
