using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    public float phaseChangeDuration = 10f;
    private float phaseTimer = 0f;
    private int currentPhase = 0;
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float shootInterval = 2f;
    private float shootTimer = 0f;

    private void Start()
    {
        health = 500; // Boss is very tough
        damage = 30;
        moveSpeed = 1.5f;
    }

    protected override void Update()
    {
        base.Update();

        if (isStunned) return;

        phaseTimer += Time.deltaTime;
        if (phaseTimer >= phaseChangeDuration)
        {
            ChangePhase();
        }

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    protected override void Move()
    {
        if (isStunned) return;

        switch (currentPhase)
        {
            case 0:
                // Phase 1: Move in a figure-eight pattern
                float t = Time.time;
                float x = Mathf.Sin(t) * 3;
                float y = Mathf.Sin(t * 2) * 1.5f;
                transform.position = new Vector3(x, y, 0) + transform.position;
                break;
            case 1:
                // Phase 2: Chase the player
                Transform player = GameObject.FindGameObjectWithTag("Player").transform;
                Vector2 direction = (player.position - transform.position).normalized;
                transform.Translate(direction * moveSpeed * Time.deltaTime);
                break;
        }
    }

    private void ChangePhase()
    {
        currentPhase = (currentPhase + 1) % 2;
        phaseTimer = 0f;
    }

    private void Shoot()
    {
        // Shoot in 8 directions
        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45 * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
            
            // Pass the damage to the projectile
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetDamage(damage);
            }
        }
    }

    public override void Stun()
    {
        base.Stun();
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}