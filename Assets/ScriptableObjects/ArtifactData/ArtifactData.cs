using UnityEngine;

public enum ArtifactType { 
    DataCrystal, 
    TechFragment, 
    RoyalInsignia, 
    ElementalCore,
}

[CreateAssetMenu(fileName = "New Artifact", menuName = "Atlantis Ascent/Artifact Data")]
public class ArtifactData: ScriptableObject {
    public string artifactName;
    public ArtifactType type;
    public Sprite sprite;

    [TextArea(3, 10)]
    public string description;
    
    public int scoreValue;
}
