using System.Collections;
using Core;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 30f;
    public float maxSpeed = 100f;
    public float horizontalDrift = 0.5f;
    public float driftFrequency = 1f;
    
    public int maxHealth = 100;

    // Blast ability parameters
    public float blastRadius = 3f;
    public float blastCooldown = 2f;

    private Rigidbody2D rb;
    private float screenWidth;
    private float screenHeight;
    private float playerWidth;
    private float playerHeight;
    private float minY; // Minimum Y position the player can reach

    private int currentHealth;
    private bool isMovementStopped = false;
    private float lastBlastTime = -Mathf.Infinity;

    private float originalMoveSpeed;
    private bool isInvincible = false;
    private bool hasMagnet = false;
    private float magnetRadius = 0f;
    private int extraBlasts = 0;

    private CameraController cameraController;

    public float horizontalBoundary = 4.5f; // Half of the zone width

    [SerializeField] private LayerMask blastAffectedLayers;
    [SerializeField] private GameObject blastEffectPrefab;
    [SerializeField] private float blastEffectDuration = 0.5f;
    [SerializeField] private float blastAngle = 30f; // Angle of the blast cone
    
    private void Start()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;

        Collider2D collider = GetComponent<Collider2D>();
        playerWidth = collider.bounds.size.x;
        playerHeight = collider.bounds.size.y;

        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        // Adjust these values for better underwater movement
        // rb.gravityScale = 0.1f;
        // rb.drag = 0.5f;
        // rb.angularDrag = 0.5f;

        minY = transform.position.y;

        originalMoveSpeed = moveSpeed;

        cameraController = Camera.main.GetComponent<CameraController>();
        if (cameraController == null)
        {
            Debug.LogError("CameraController not found on the main camera!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 blastDirection = transform.right * (true ? 1 : -1);
            // Vector2 blastDirection = transform.right * (isFacingRight ? 1 : -1);
            ActivateBlast(blastDirection);
        }
    }
    private void FixedUpdate()
    {
        if (!isMovementStopped)
        {
            HandleMovement();
        }

        if (hasMagnet) {
            AttractionMagnet();
        }

        ApplyDrift();
        ClampVelocity();
        ClampPosition();
    }

    private void AttractionMagnet()
    {
        Collider2D[] attractedObjects = Physics2D.OverlapCircleAll(transform.position, magnetRadius);
        foreach (Collider2D obj in attractedObjects)
        {
            if (obj.CompareTag("OxygenBubble") || obj.CompareTag("PowerUp"))
            {
                Vector2 direction = (transform.position - obj.transform.position).normalized;
                obj.GetComponent<Rigidbody2D>().AddForce(direction * 5f);
            }
        }
    }

    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }
        // Apply movement as a force
        rb.AddForce(movement * moveSpeed);
    }

    private void ApplyDrift()
    {
        float drift = Mathf.Sin(Time.time * driftFrequency) * horizontalDrift;
        rb.AddForce(new Vector2(drift, 0), ForceMode2D.Force);
    }

    private void ClampVelocity()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }
    
    private void ClampPosition()
    {
        // Screen wrapping (horizontal)
        if (transform.position.x > screenWidth / 2 + playerWidth / 2)
        {
            transform.position = new Vector3(-screenWidth / 2 - playerWidth / 2, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -screenWidth / 2 - playerWidth / 2)
        {
            transform.position = new Vector3(screenWidth / 2 + playerWidth / 2, transform.position.y, transform.position.z);
        }

        // Vertical clamping (only prevent going below minY)
        if (transform.position.y < minY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        }

        // Update camera bounds based on player position
        UpdateCameraBounds();
    }

    private void UpdateCameraBounds()
    {
        if (cameraController != null)
        {
            // Update the camera's minimum Y position based on the player's position
            cameraController.MinY = Mathf.Max(cameraController.MinY, transform.position.y - screenHeight / 2);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("StaticObstacle"))
        {
            TakeDamage(1);
        }
        else if (collision.gameObject.CompareTag("DestructibleObstacle") || collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible) {
            currentHealth -= damage;
            Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Implement game over logic here
    }

    private void ActivateBlast(Vector2 direction)
    {
        lastBlastTime = Time.time;

        // Spawn and orient visual effect
        GameObject blastEffect = Instantiate(blastEffectPrefab, transform.position, Quaternion.identity);
        blastEffect.transform.up = direction; // Orient the effect in the blast direction
        Destroy(blastEffect, blastEffectDuration);

        // Calculate blast area
        Vector2 blastStart = (Vector2)transform.position;
        Vector2 blastEnd = blastStart + direction * blastRadius;

        // Use OverlapAreaNonAlloc for a cone-shaped blast
        Collider2D[] hitColliders = new Collider2D[20]; // Adjust array size as needed
        int numColliders = Physics2D.OverlapAreaNonAlloc(blastStart, blastEnd, hitColliders, blastAffectedLayers);

        for (int i = 0; i < numColliders; i++)
        {
            Collider2D hitCollider = hitColliders[i];
        
            // Check if the collider is within the blast angle
            Vector2 toCollider = hitCollider.bounds.center - transform.position;
            float angle = Vector2.Angle(direction, toCollider);
        
            if (angle <= blastAngle / 2)
            {
                if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    Enemy enemy = hitCollider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.Stun();
                    }
                }
                else if (hitCollider.gameObject.layer == LayerMask.NameToLayer("DestructibleObstacle"))
                {
                    Destroy(hitCollider.gameObject);
                }
            }
        }

        // AudioManager.Instance.PlaySoundEffect("BlastSound");
        // CameraShake.Instance.ShakeCamera(0.2f, 0.5f);
    }


    private void OnDrawGizmosSelected()
    {
        // Visualize the blast radius in the Scene view
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        StartCoroutine(SpeedBoostCoroutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        moveSpeed *= multiplier;
        yield return new WaitForSeconds(duration);
        RemoveSpeedBoost();
    }

    public void RemoveSpeedBoost()
    {
        moveSpeed = originalMoveSpeed;
    }


    public void ApplyInvincibility(float duration)
    {
        StartCoroutine(InvincibilityCoroutine(duration));
    }

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;
        // TODO: Add visual effect for invincibility
        yield return new WaitForSeconds(duration);
        RemoveInvincibility();
    }

    public void RemoveInvincibility()
    {
        isInvincible = false;
        // TODO: Remove visual effect for invincibility
    }

    public void ApplyMagnet(float radius, float duration)
    {
        StartCoroutine(MagnetCoroutine(radius, duration));
    }

    private IEnumerator MagnetCoroutine(float radius, float duration)
    {
        hasMagnet = true;
        magnetRadius = radius;
        yield return new WaitForSeconds(duration);
        RemoveMagnet();
    }

    public void RemoveMagnet()
    {
        hasMagnet = false;
        magnetRadius = 0f;
    }

    public void ApplyMultiBlast(int blasts, float duration)
    {
        StartCoroutine(MultiBlastCoroutine(blasts, duration));
    }

    private IEnumerator MultiBlastCoroutine(int blasts, float duration)
    {
        extraBlasts = blasts;
        yield return new WaitForSeconds(duration);
        RemoveMultiBlast();
    }

    public void RemoveMultiBlast()
    {
        extraBlasts = 0;
    }

    public void UpdateHealthDisplay() 
    {
        GameManager.Instance.UIManager.UpdateHealthDisplay(currentHealth, maxHealth);
    }
}