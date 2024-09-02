using UnityEngine;

public class DestructibleObstacle : Obstacle {
    public GameObject destructionEffect;

     protected override void Start() {
        base.Start();

        isDestructible = true;
    }

    public override void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.CompareTag("Player")) {
            DestroyObstacle();
        }
    }

    public override void TakeDamage(int damage) {
        base.TakeDamage(damage);

        
        if (hitPoints <= 0) {
            DestroyObstacle();
        }
    }

    private void DestroyObstacle() {
        if (destructionEffect != null) {
            Instantiate(destructionEffect, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}