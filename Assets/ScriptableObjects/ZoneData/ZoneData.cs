using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Zone", menuName = "Atlantis Ascent/Zone Data")]
public class ZoneData : ScriptableObject
{
    public string zoneName;
    public float startHeight;
    public float endHeight;
    public Color ambientLight;

    public List<GameObject> obstaclePrefabs;
    public List<GameObject> enemyPrefabs;
    public List<GameObject> artifactPrefabs;
    public Sprite backgroundSprite;
}