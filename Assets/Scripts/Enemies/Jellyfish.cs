using UnityEngine;

public class Jellyfish : Enemy
{
    public float verticalAmplitude = 1f;
    public float verticalFrequency = 1f;
    public float horizontalAmplitude = 0.5f;
    public float horizontalFrequency = 0.5f;
    private Vector3 startPosition;

    public float detectionRange = 3f;
    public float attackRange = 1f;
    public float attackCooldown = 2f;
    private float lastAttackTime;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform player;

    private void Start()
    {
        startPosition = transform.position;
        health = 50; // Jellyfish are weaker
        damage = 5;
        moveSpeed = 1f; // Slower than other enemies

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastAttackTime = -attackCooldown; // Allow immediate attack
    }

    protected override void Move()
    {
        if (isStunned) return;

        Vector2 movement;
        bool isAttacking = false;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Move towards the player
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            movement = directionToPlayer * moveSpeed * Time.deltaTime;

            if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                isAttacking = true;
            }
        }
        else
        {
            // Normal wave-like movement
            float newY = startPosition.y + Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;
            float newX = startPosition.x + Mathf.Sin(Time.time * horizontalFrequency) * horizontalAmplitude;
            Vector3 newPosition = new Vector3(newX, newY, transform.position.z);
            movement = (newPosition - transform.position).normalized * moveSpeed * Time.deltaTime;
        }

        transform.Translate(movement);

        UpdateAnimation(movement, isAttacking);

        if (movement.x > 0)
            spriteRenderer.flipX = false;
        else if (movement.x < 0)
            spriteRenderer.flipX = true;

        float verticalMovement = player.position.y - transform.position.y;
        transform.Translate(Vector2.up * verticalMovement * Time.deltaTime);
    }

    private void UpdateAnimation(Vector2 movement, bool isAttacking)
    {
        if (animator != null)
        {
            animator.SetBool("IsMoving", movement.magnitude > 0.1f);
            // animator.SetBool("IsAttacking", isAttacking);
        }
    }

    private void Attack()
    {
        lastAttackTime = Time.time;
        
        // Play attack animation
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Check if player is in attack range and deal damage
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
            }
        }
    }

    public override void Stun()
    {
        base.Stun();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.gray;
        }
        if (animator != null)
        {
            animator.SetBool("IsStunned", true);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
       
    }
}