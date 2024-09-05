using UnityEngine;
namespace Abilities
{
    public class Blast : MonoBehaviour
    {
        private int damage;
        private float range;
        private LayerMask enemyLayer;
        private Vector3 startPosition;

        public void Initialize(int damage, float range, LayerMask enemyLayer)
        {
            this.damage = damage;
            this.range = range;
            this.enemyLayer = enemyLayer;
            startPosition = transform.position;
        }

        private void Update()
        {
            if (Vector3.Distance(startPosition, transform.position) > range)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
        }
    }
}