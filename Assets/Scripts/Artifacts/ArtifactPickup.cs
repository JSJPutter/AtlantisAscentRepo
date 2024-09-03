using ScriptableObjects.ArtifactData;
using UnityEngine;

public class ArtifactPickup : MonoBehaviour
{
    public Artifact artifact;

    private void Start()
    {
        // Set the sprite of this object to the artifact's icon
        GetComponent<SpriteRenderer>().sprite = artifact.icon;
    }
}