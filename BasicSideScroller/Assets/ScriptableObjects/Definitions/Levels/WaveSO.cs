using UnityEngine;

public enum SpawnType
{
    Timer,                  // Timer wave spawns will start their timer once the previous wave spawns.
    EndOfWave,              // EndOfWave wave spawns will start once the previous wave is complete. Can still use a timer to spawn after a short delay.
    HealthPercentage        // HealthPercentage wave spawns will spawn after the key unit of the wave reaches a certain percentage.
};

public enum SpawnLocation
{
    SpawnBox,
    SpawnPoint
};

public enum EnemyWaveType
{
    Normal,
    Captain,
    Boss
}

[CreateAssetMenu(fileName="New Wave",menuName="Waves/BasicWave")]
public class WaveSO : ScriptableObject
{
    public SpawnType waveSpawnType;
    public SpawnLocation spawnLocationType;
    public EnemyWaveType enemyWaveType;

    public float TimeUntilSpawn;

    public GameObject[] enemies;

    public float spawnBoxPosX;
    public float spawnBoxPosY;
    public float spawnBoxWidth;
    public float spawnBoxHeight;
}
