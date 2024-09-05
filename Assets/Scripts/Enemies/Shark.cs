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

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        patrolStartPosition = transform.position;
        health = 150; // Sharks are tougher
        damage = 20;
        moveSpeed = patrolSpeed;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Move()
    {
        if (isStunned) return;

        Vector2 movement = Vector2.zero;
        bool isChasing = false;

        if (Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            // Chase the player
            Vector2 direction = (player.position - transform.position).normalized;
            movement = direction * chaseSpeed * Time.deltaTime;
            isChasing = true;
        }
        else
        {
            // Patrol behavior
            if (movingRight)
            {
                movement = Vector2.right * patrolSpeed * Time.deltaTime;
                if (transform.position.x >= patrolStartPosition.x + patrolDistance)
                    movingRight = false;
            }
            else
            {
                movement = Vector2.left * patrolSpeed * Time.deltaTime;
                if (transform.position.x <= patrolStartPosition.x - patrolDistance)
                    movingRight = true;
            }
        }

        // Move the shark
        transform.Translate(movement);

        // Update animations
        UpdateAnimation(movement, isChasing);

        // Flip the sprite based on movement direction
        if (movement.x > 0 && !movingRight)
            Flip();
        else if (movement.x < 0 && movingRight)
            Flip();

        // Make the shark ascend with the player
        float verticalMovement = player.position.y - transform.position.y;
        transform.Translate(Vector2.up * verticalMovement * Time.deltaTime);
    }

    private void UpdateAnimation(Vector2 movement, bool isChasing)
    {
        if (animator != null)
        {
            animator.SetBool("IsMoving", movement.magnitude > 0.1f);
            animator.SetBool("IsChasing", isChasing);
        }
    }

    private void Flip()
    {
        movingRight = !movingRight;
        spriteRenderer.flipX = !movingRight;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            // Play attack animation
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
        }
    }
}