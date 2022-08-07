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

[System.Serializable]
public struct SizePostion2D
{
    public float width;
    public float height;
    public Vector2 position;   
}

[CreateAssetMenu(fileName="New Wave",menuName="Waves/BasicWave")]
public class WaveSO : ScriptableObject
{
    public SpawnType waveSpawnType;
    public SpawnLocation spawnLocationType;
    public EnemyWaveType enemyWaveType;

    public float TimeUntilSpawn;
    public float SuccessiveUnitDelay;   // If not all enemies in a wave should be spawned at once, can be delayed (in seconds)

    public GameObject[] enemies;
    public SizePostion2D[] spawns;      // List of spawn location and size (if a spawn box)

    // Will be unused. Keeping for now to not break the game.
    public float spawnBoxPosX;
    public float spawnBoxPosY;
    public float spawnBoxWidth;
    public float spawnBoxHeight;
}
