using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : Enemy
{
    private Transform player;
    public float detectionRange = 5f;
    public float chaseSpeed = 3f;
    public float patrolSpeed = 1.5f;
    private Vector3 patrolStartPosition;
    private float patrolDistance = 3f;
    private bool movingRight = true;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        patrolStartPosition = transform.position;
        health = 150; // Sharks are tougher
        damage = 20;
        moveSpeed = patrolSpeed;
    }

    protected override void Move()
    {
        if (isStunned) return;

        if (Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            // Chase the player
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * chaseSpeed * Time.deltaTime);
            
            // Flip the sprite based on movement direction
            if (direction.x > 0 && !movingRight)
                Flip();
            else if (direction.x < 0 && movingRight)
                Flip();
        }
        else
        {
            // Patrol behavior
            if (movingRight)
            {
                transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);
                if (transform.position.x >= patrolStartPosition.x + patrolDistance)
                    movingRight = false;
            }
            else
            {
                transform.Translate(Vector2.left * patrolSpeed * Time.deltaTime);
                if (transform.position.x <= patrolStartPosition.x - patrolDistance)
                    movingRight = true;
            }
        }
    }

    private void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
