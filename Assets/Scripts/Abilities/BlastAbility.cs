using Abilities;
using UnityEngine;

public class BlastAbility : MonoBehaviour
{
    public float blastSpeed = 10f;
    public float blastRange = 10f;
    public int blastDamage = 50;
    public LayerMask enemyLayer;
    public GameObject blastPrefab;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBlast();
        }
    }

    private void ShootBlast()
    {
        Vector2 shootDirection = playerController.IsFacingRight ? Vector2.right : Vector2.left;
        Vector3 spawnPosition = transform.position + (Vector3)shootDirection * 0.5f; // Offset to avoid self-collision

        GameObject blastObject = Instantiate(blastPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D blastRb = blastObject.GetComponent<Rigidbody2D>();
        
        if (blastRb != null)
        {
            blastRb.velocity = shootDirection * blastSpeed;
        }

        Blast blastComponent = blastObject.GetComponent<Blast>();
        if (blastComponent == null)
        {
            blastComponent = blastObject.AddComponent<Blast>();
        }
        blastComponent.Initialize(blastDamage, blastRange, enemyLayer);

        // TODO: Add sound effect for shooting
        // AudioManager.Instance.PlaySound("BlastSound");
    }
}
