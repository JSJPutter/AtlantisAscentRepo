using ScriptableObjects.ArtifactData;
using UnityEngine;

public class ArtifactPickup : MonoBehaviour
{
    public Artifact artifact;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = artifact.icon;
    }
}