using UnityEngine;

public enum SpawnType
{
    Timer,
    EndOfWave,
    HealthPercentage
};

public enum SpawnLocation
{
    SpawnBox,
    SpawnPoint
};

[CreateAssetMenu(fileName="New Wave",menuName="Waves/BasicWave")]
public class WaveSO : ScriptableObject
{
    public SpawnType waveSpawnType;
    public SpawnLocation spawnLocationType;

    public GameObject[] enemies;
    //public GameObject spawnBox;

    public float spawnBoxPosX;
    public float spawnBoxPosY;
    public float spawnBoxWidth;
    public float spawnBoxHeight;
}
