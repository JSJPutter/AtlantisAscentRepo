using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public float duration = 5f;
    public string powerUpName;

    protected PlayerController player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                Apply();
                Destroy(gameObject);
            }
        }
    }

    protected abstract void Apply();
    public abstract void Remove();
}
