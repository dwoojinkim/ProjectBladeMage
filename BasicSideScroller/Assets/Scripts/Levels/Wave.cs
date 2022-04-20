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

    [SerializeField] private SpawnType WaveSpawnType;

    [SerializeField] private GameObject[] enemies;

    private float timer = 0;
 
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3)
        {
            Debug.Log("Spawning Wave!");
            SpawnWave();
            timer = -100;
        }

        if (IsWaveOver())
            Debug.Log("WAVE IS OVER!!!");
    }

    private void SpawnWave()
    {
        if (enemies.Length > 0)
        {
            foreach (GameObject e in enemies)
            {
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
