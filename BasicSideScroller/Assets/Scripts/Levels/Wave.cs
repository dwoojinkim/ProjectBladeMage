using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    enum SpawnType
    {
        Timer,
        EndOfWave,
        HealthPercentage
    };

    enum SpawnLocation
    {
        SpawnBox,
        SpawnPoint
    }

    private enum WaveState
    {
        Idle,
        WaveStarted,
        WaveComplete
    };

    [SerializeField] private SpawnType WaveSpawnType;
    [SerializeField] private SpawnLocation SpawnLocationType;

    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject spawnBox;

    private float timer = 0;
    private WaveState waveState;
 
    // Start is called before the first frame update
    void Awake()
    {
        waveState = WaveState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (waveState == WaveState.Idle || waveState == WaveState.WaveComplete)
        {
            timer += Time.deltaTime;
            if (timer > 3)
            {
                Debug.Log("Spawning Wave!");
                waveState = WaveState.WaveStarted;
                SpawnWave();
            }
        }

        if (waveState == WaveState.WaveStarted && IsWaveOver())
        {
            Debug.Log("WAVE IS OVER!!!");
            waveState = WaveState.WaveComplete;
            timer = 0;
        }
    }

    private void SpawnWave()
    {
        if (enemies.Length > 0)
        {
            foreach (GameObject e in enemies)
            {
                if (SpawnLocationType == SpawnLocation.SpawnBox)
                {
                    float leftBound = spawnBox.transform.position.x - spawnBox.GetComponent<SpriteRenderer>().bounds.size.x/2f;
                    float rightBound = spawnBox.transform.position.x + spawnBox.GetComponent<SpriteRenderer>().bounds.size.x/2f;
                    float bottomBound = spawnBox.transform.position.y - spawnBox.GetComponent<SpriteRenderer>().bounds.size.y/2f;
                    float topBound = spawnBox.transform.position.y + spawnBox.GetComponent<SpriteRenderer>().bounds.size.y/2f;

                    Vector2 spawnPos = new Vector2(Random.Range(leftBound, rightBound), Random.Range(bottomBound, topBound));
                    e.GetComponent<Enemy>().SpawnEnemy(spawnPos);
                }
                else
                    e.GetComponent<Enemy>().SpawnEnemy();
            }
        }
    }

    public bool IsWaveOver()
    {
        foreach (GameObject e in enemies)
        {
            if (e.GetComponent<Enemy>().IsAlive)
                return false;
        }

        return true;
    }
}
