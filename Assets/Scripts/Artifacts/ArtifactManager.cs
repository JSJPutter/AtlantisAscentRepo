using System.Collections.Generic;
using Core;
using ScriptableObjects.ArtifactData;
using UnityEngine;

namespace Artifacts
{
    public class ArtifactManager : MonoBehaviour
    {
        public static ArtifactManager Instance { get; private set; }

        public List<ScriptableObjects.ArtifactData.Artifact> allArtifacts = new List<ScriptableObjects.ArtifactData.Artifact>();
        public List<Artifact> collectedArtifacts = new List<Artifact>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void CollectArtifact(Artifact artifact)
        {
            if (!artifact.isCollected)
            {
                artifact.isCollected = true;
                collectedArtifacts.Add(artifact);
                GameManager.Instance.AddScore(artifact.pointValue);
                // Trigger UI update
                ArtifactDisplay.Instance.UpdateDisplay();
                // You might want to trigger other events here, like unlocking upgrades
            }
        }

        public bool IsArtifactCollected(Artifact artifact)
        {
            return collectedArtifacts.Contains(artifact);
        }
    }
}