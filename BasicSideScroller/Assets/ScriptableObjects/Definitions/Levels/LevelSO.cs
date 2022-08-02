using UnityEngine;

[CreateAssetMenu(fileName = "New Basic Level", menuName ="Levels/BasicLevel")]
public class LevelSO : ScriptableObject
{
    public string levelName;
    public GameObject levelPrefab;
    public Sprite portalPreview;
}
