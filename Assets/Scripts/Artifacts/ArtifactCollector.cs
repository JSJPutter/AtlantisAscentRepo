using UnityEngine;

namespace Artifacts
{
    public class ArtifactCollector : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            ArtifactPickup artifactPickup = collision.GetComponent<ArtifactPickup>();
            if (artifactPickup != null)
            {
                ArtifactManager.Instance.CollectArtifact(artifactPickup.artifact);
                Destroy(artifactPickup.gameObject);
            }
        }
    }
}