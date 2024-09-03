using UnityEngine;

namespace ScriptableObjects.ArtifactData
{
    [CreateAssetMenu(fileName = "New Artifact", menuName = "Atlantis Ascent/Artifact")]
    public class Artifact : ScriptableObject
    {
        public string artifactName;
        public Sprite icon;
        [TextArea(3, 10)]
        public string description;
        public int pointValue;
        public ArtifactType type;
        public bool isCollected = false;

        public enum ArtifactType
        {
            Common,
            Rare,
            Legendary
        }
    }
}
