using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    public int hitPoints = 1;
    public bool isDestructible = false;
    public float sinkForce = 0.3f;
    public float horizontalWaveMagnitude = 0.5f;
    public float horizontalWaveFrequency = 1f;

    protected Rigidbody2D rb;
    private float startX;
    private float timeSinceSpawn;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = .4f;  // Allow gravity to affect the object
        startX = transform.position.x;
        timeSinceSpawn = 0f;
    }

    protected virtual void Update()
    {
        if (IsOutOfView())
        {
            Destroy(gameObject);
        }
    }

    protected virtual void FixedUpdate()
    {
        ApplyBuoyantBehavior();
    }

    protected void ApplyBuoyantBehavior()
    {
        timeSinceSpawn += Time.fixedDeltaTime;

        // Apply downward force to make the obstacle sink
        rb.AddForce(Vector2.down * sinkForce);

        // Calculate and apply horizontal wave motion
        float horizontalOffset = Mathf.Sin(timeSinceSpawn * horizontalWaveFrequency) * horizontalWaveMagnitude;
        Vector2 horizontalForce = new Vector2(horizontalOffset - (rb.position.x - startX), 0);
        rb.AddForce(horizontalForce);
    }

    protected bool IsOutOfView()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.y < 0 || screenPoint.x < 0;
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(1);
            }
            if (isDestructible)
            {
                TakeDamage(1);
            }
        }
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDestructible)
        {
            hitPoints -= damage;
            if (hitPoints <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}