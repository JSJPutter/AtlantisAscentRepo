using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int health = 100;
    public int damage = 10;
    protected bool isStunned = false;
    protected float stunDuration = 2f;
    protected float stunTimer = 0f;
    public Action<GameObject> OnDestroyed; // Event for when the enemy is destroyed
    protected virtual void Update()
    {
        if (isStunned)
        {
            stunTimer += Time.deltaTime;
            if (stunTimer >= stunDuration)
            {
                isStunned = false;
                stunTimer = 0f;
            }
            return;
        }

        Move();
    }

    protected abstract void Move();

    public virtual void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }
   

    private void OnDestroy()
    {
        if (OnDestroyed != null)
        {
            OnDestroyed(gameObject);
        }
    }
    public virtual void Stun()
    {
        isStunned = true;
        stunTimer = 0f;
    }

    protected virtual void Die()
    {
        Core.GameManager.Instance.AddScore(2);
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
            }
        }
    }
}