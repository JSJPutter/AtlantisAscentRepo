using UnityEngine;

public class SineWaveEnemy : Enemy
{
    public float amplitude = 2f;
    public float frequency = 1f;

    private Vector3 startPosition;
    private float timeSinceSpawn;

    private void Start()
    {
        startPosition = transform.position;
        timeSinceSpawn = 0f;
    }

    protected override void Move()
    {
        timeSinceSpawn += Time.deltaTime;
        
        float horizontalOffset = Mathf.Sin(timeSinceSpawn * frequency) * amplitude;
        Vector3 newPosition = startPosition + new Vector3(horizontalOffset, -moveSpeed * timeSinceSpawn, 0);
        
        transform.position = newPosition;

        // Destroy if off-screen
        if (transform.position.y < -Camera.main.orthographicSize - 1)
        {
            Destroy(gameObject);
        }
    }
}