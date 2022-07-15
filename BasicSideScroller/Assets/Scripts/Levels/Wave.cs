using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{

    // DELETE THESE IF GAME RUNS NORMALLY AS THEY ARE ALREADY A PART
    // OF THE WAVE SRIPTABLE OBJECT
    /*
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
    }*/

    private enum WaveState
    {
        Idle,
        WaveStarted,
        WaveComplete
    };

    [SerializeField] private WaveSO waveData;

    private SpawnType waveSpawnType;
    private SpawnLocation spawnLocationType;

    private List<GameObject> enemies = new List<GameObject>();
    //private GameObject spawnBox;
    private Vector2 spawnBoxDimensions;
    private Vector2 spawnBoxPosition;

    private float timer = 0;
    private WaveState waveState;
 
    // Start is called before the first frame update
    void Awake()
    {
        waveState = WaveState.Idle;

        waveSpawnType = waveData.waveSpawnType;
        spawnLocationType = waveData.spawnLocationType;
        
        //spawnBox = waveData.spawnBox;
        spawnBoxDimensions.x = waveData.spawnBoxWidth;
        spawnBoxDimensions.y = waveData.spawnBoxHeight;

        spawnBoxPosition.x = waveData.spawnBoxPosX;
        spawnBoxPosition.y = waveData.spawnBoxPosY;

        enemies = InstantiateEnemies(waveData.enemies);
    }

    // Update is called once per frame
    void Update()
    {
        if (waveState == WaveState.Idle || waveState == WaveState.WaveComplete)
        {
            timer += Time.deltaTime;
            if (timer > 1)
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

    // Copied snippet from user 'chilemanga' (https://answers.unity.com/questions/461588/drawing-a-bounding-box-similar-to-box-collider.html)
    // It has been modified so the gizmos will be drawn from the ScriptableObject data so it can be seen outside of runtime.
    // I should be able to modify this to loop through multiple spawnboxes
    void OnDrawGizmos() 
    {
         Gizmos.color = Color.yellow;
         float wHalf = (waveData.spawnBoxWidth * .5f);
         float hHalf = (waveData.spawnBoxHeight * .5f);
         Vector3 topLeftCorner = new Vector3 (waveData.spawnBoxPosX - wHalf, waveData.spawnBoxPosY + hHalf, 1f);
         Vector3 topRightCorner = new Vector3 (waveData.spawnBoxPosX + wHalf, waveData.spawnBoxPosY + hHalf, 1f);
         Vector3 bottomLeftCorner = new Vector3 (waveData.spawnBoxPosX - wHalf, waveData.spawnBoxPosY - hHalf, 1f);
         Vector3 bottomRightCorner = new Vector3 (waveData.spawnBoxPosX + wHalf, waveData.spawnBoxPosY - hHalf, 1f);
         Gizmos.DrawLine (topLeftCorner, topRightCorner);
         Gizmos.DrawLine (topRightCorner, bottomRightCorner);
         Gizmos.DrawLine (bottomRightCorner, bottomLeftCorner);
         Gizmos.DrawLine (bottomLeftCorner, topLeftCorner);
     }

    // Instantiates the list of enemy prefabs from the Wave Scriptable Object
    private List<GameObject> InstantiateEnemies(GameObject[] enemyPrefabList)
    {
        List<GameObject> enemyList = new List<GameObject>();
        GameObject enemy;

        for(int i = 0; i < enemyPrefabList.Length; i++)
        {
            enemy = Instantiate(enemyPrefabList[i]);
            enemy.SetActive(false);
            enemyList.Add(enemy);
        }

        return enemyList;
    }
    
    private void SpawnWave()
    {
        if (enemies.Count > 0)
        {
            foreach (GameObject e in enemies)
            {
                if (spawnLocationType == SpawnLocation.SpawnBox)
                {
                    float leftBound = spawnBoxPosition.x - spawnBoxDimensions.x/2f;
                    float rightBound = spawnBoxPosition.x + spawnBoxDimensions.x/2f;
                    float bottomBound = spawnBoxPosition.y - spawnBoxDimensions.y/2f;
                    float topBound = spawnBoxPosition.y + spawnBoxDimensions.y/2f;

                    Vector2 spawnPos = new Vector2(Random.Range(leftBound, rightBound), Random.Range(bottomBound, topBound));
                    e.GetComponent<Enemy>().SpawnEnemy(spawnPos);
                    e.SetActive(true);
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
