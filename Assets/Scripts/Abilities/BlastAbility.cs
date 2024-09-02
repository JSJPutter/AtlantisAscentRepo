using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BlastAbility : MonoBehaviour
{
    public float blastRadius = 3f;
    public float blastCooldown = 2f;
    public LayerMask enemyLayer;

    private float lastBlastTime = -Mathf.Infinity;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastBlastTime >= blastCooldown)
        {
            ActivateBlast();
        }
    }

    private void ActivateBlast()
    {
        lastBlastTime = Time.time;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, blastRadius, enemyLayer);

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Stun();
            }
        }

        // TODO: Add visual effects for the blast
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}