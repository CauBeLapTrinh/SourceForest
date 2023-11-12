using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public Transform pointA, pointB;
    Vector3 currentTarget;

    Animator animator;

    float distance;
    SpriteRenderer spriteRenderer;
    bool canMove;
    // Update is called once per frame

    private void Start()
    {
        canMove = true;
        currentTarget = pointA.position;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 4 && player.transform.position.x > pointA.position.x && player.transform.position.x < pointB.position.x)
        {
            followPlayer();
        }

        else if (canMove)
        {
            autoMovement();
        }
        else if (!canMove)
        {
            animator.SetFloat("Walk", 0);
        }
    }
    void autoMovement()
    {
        if (currentTarget == pointA.position) 
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        if (transform.position == pointA.position)
        {
            currentTarget = pointB.position;
            
            canMove = false;
            Invoke("changeMove", 1);
        }
        else if (transform.position == pointB.position)
        {
            currentTarget = pointA.position;
            canMove = false;
            Invoke("changeMove", 1);
        }

        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
        animator.SetFloat("Walk", 1);
    }

    void followPlayer()
    {
        if (transform.position.x - player.transform.position.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        animator.SetFloat("Walk", 1);
    }

    void changeMove()
    {
        canMove = true;
    }
}
