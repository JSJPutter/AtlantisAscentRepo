using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Artifacts;

public class ArtifactDisplay : MonoBehaviour
{
    public static ArtifactDisplay Instance { get; private set; }

    public GameObject artifactIconPrefab;
    public Transform artifactContainer;

    private List<GameObject> artifactIcons = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateDisplay()
    {
        // Clear existing icons
        foreach (var icon in artifactIcons)
        {
            Destroy(icon);
        }
        artifactIcons.Clear();

        // Create new icons for collected artifacts
        foreach (var artifact in ArtifactManager.Instance.collectedArtifacts)
        {
            GameObject newIcon = Instantiate(artifactIconPrefab, artifactContainer);
            newIcon.GetComponent<Image>().sprite = artifact.icon;
            artifactIcons.Add(newIcon);
        }
    }
}