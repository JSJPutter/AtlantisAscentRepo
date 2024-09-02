using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : Enemy
{
    public float verticalAmplitude = 1f;
    public float verticalFrequency = 1f;
    public float horizontalAmplitude = 0.5f;
    public float horizontalFrequency = 0.5f;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        health = 50; // Jellyfish are weaker
        damage = 5;
        moveSpeed = 1f; // Slower than other enemies
    }

    protected override void Move()
    {
        if (isStunned) return;

        float newY = startPosition.y + Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;
        float newX = startPosition.x + Mathf.Sin(Time.time * horizontalFrequency) * horizontalAmplitude;
        transform.position = new Vector3(newX, newY, transform.position.z);
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    public override void Stun()
    {
        base.Stun();
        // Add visual effect for stunned jellyfish
        GetComponent<SpriteRenderer>().color = Color.gray;
    }
}