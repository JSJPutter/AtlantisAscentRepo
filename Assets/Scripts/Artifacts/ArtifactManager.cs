using System.Collections.Generic;
using Core;
using ScriptableObjects.ArtifactData;
using UnityEngine;

namespace Artifacts
{
    public class ArtifactManager : MonoBehaviour
    {
        public static ArtifactManager Instance { get; private set; }

        public List<Artifact> allArtifacts = new List<Artifact>();
        public List<Artifact> collectedArtifacts = new List<Artifact>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
              //  DontDestroyOnLoad(gameObject);
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
                ArtifactDisplay.Instance.UpdateDisplay();
            }
        }

        public bool IsArtifactCollected(Artifact artifact)
        {
            return collectedArtifacts.Contains(artifact);
        }
    }
}