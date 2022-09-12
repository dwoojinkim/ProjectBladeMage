using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public delegate void Notify();  // delegate

public class Wave : MonoBehaviour
{
    public enum WaveState
    {
        Standby,
        Initiated,
        WaveStarted,
        WaveSpawnComplete,       // When Wave has completed spawning. Needed to delayed spawns
        WaveComplete
    };

    public WaveState CurrentWaveState {get; private set;}
    public event Notify WaveStart;      // event
    public event Notify WaveComplete;   // Wave complettion event

    [SerializeField] private WaveSO waveData;
    [SerializeField] private WaveRuntimeSet activeWaveSet;
    [SerializeField] private GameObject spawnPortalPrefab;

    private SpawnType waveSpawnType;
    private SpawnLocation spawnLocationType;
    private EnemyWaveType enemyWaveType;
    private SpawnOrder enemySpawnOrder;

    private List<GameObject> spawnEnemies = new List<GameObject>();             // List of enemies for 1 particular spawn point/box.
    private List<List<GameObject>> enemies = new List<List<GameObject>>();      // List of list of spawnEnemies. Full list of enemies.
    private SizePostion2D[] spawnLocations;
    private List<GameObject> spawnPortals = new List<GameObject>();
    private Vector2 spawnBoxDimensions;
    private Vector2 spawnBoxPosition;

    private float timer;
    private float timeUntilSpawn;
    private float successiveUnitDelay;

    private bool waveTrigger = false;
 
 
    // Start is called before the first frame update
    void Awake()
    {
        CurrentWaveState = WaveState.Standby;

        waveSpawnType = waveData.waveSpawnType;
        spawnLocationType = waveData.spawnLocationType;
        enemyWaveType = waveData.enemyWaveType;
        enemySpawnOrder = waveData.enemySpawnOrder;
        spawnLocations = waveData.spawns;
        
        timer = 0;
        timeUntilSpawn = waveData.TimeUntilSpawn;
        successiveUnitDelay = waveData.SuccessiveUnitDelay;

        spawnBoxDimensions.x = waveData.spawnBoxWidth;
        spawnBoxDimensions.y = waveData.spawnBoxHeight;

        spawnBoxPosition.x = waveData.spawnBoxPosX;
        spawnBoxPosition.y = waveData.spawnBoxPosY;

        enemies = InstantiateEnemies(waveData.enemies);
    }

    void Start()
    {
        InstantiateSpawnPortals();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentWaveState == WaveState.Initiated) // || waveState == WaveState.WaveComplete) // It should only check WaveComplete state to restart if it's supposed to re-spawn
        {

            if (waveSpawnType == SpawnType.Timer)
            {
                timer += Time.deltaTime;

                foreach (GameObject spawnPortObj in spawnPortals)
                {
                    spawnPortObj.SetActive(true);
                    spawnPortObj.GetComponent<SpawnPortal>().StartSpawn();
                }

                if (timer > timeUntilSpawn)
                {
                    Debug.Log("Spawning Wave!");
                    CurrentWaveState = WaveState.WaveStarted;

                    StartCoroutine(SpawnWave());
                }
            }
            else if (waveSpawnType == SpawnType.Trigger)
            {
               if (waveTrigger)
                {
                    // Keeping timer so the enemies don't INSTANTLY spawn and gives player some time to realize enemies are spawning.
                    timer += Time.deltaTime;

                    foreach (GameObject spawnPortObj in spawnPortals)
                    {
                        spawnPortObj.SetActive(true);
                        spawnPortObj.GetComponent<SpawnPortal>().StartSpawn();
                    }

                    if (timer > timeUntilSpawn)
                    {
                        Debug.Log("Spawning Wave!");
                        CurrentWaveState = WaveState.WaveStarted;

                        StartCoroutine(SpawnWave());
                    }
                }
            }
        }

        if (CurrentWaveState == WaveState.WaveSpawnComplete && IsWaveOver())
        {
            Debug.Log("WAVE IS OVER!!!");
            CurrentWaveState = WaveState.WaveComplete;
            timer = 0;
            this.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        activeWaveSet.Add(this);
    }

    private void OnDisable()
    {
        activeWaveSet.Remove(this);
    }

    // Copied snippet from user 'chilemanga' (https://answers.unity.com/questions/461588/drawing-a-bounding-box-similar-to-box-collider.html)
    // It has been modified so the gizmos will be drawn from the ScriptableObject data so it can be seen outside of runtime.
    // I should be able to modify this to loop through multiple spawnboxes
    void OnDrawGizmos() 
    {
        if (waveData.spawnLocationType == SpawnLocation.SpawnBox)
        {
            if (waveData.spawns.Length != 0)
            {
                for (int i = 0; i < waveData.spawns.Length; i++)
                {
                    float boxWidth = waveData.spawns[i].width;
                    float boxHeight = waveData.spawns[i].height;
                    Vector2 boxPos = waveData.spawns[i].position;

                    Gizmos.color = Color.yellow;
                    float wHalf = (boxWidth * .5f);
                    float hHalf = (boxHeight * .5f);
                    Vector3 topLeftCorner = new Vector3 (boxPos.x - wHalf, boxPos.y + hHalf, 1f);
                    Vector3 topRightCorner = new Vector3 (boxPos.x + wHalf, boxPos.y + hHalf, 1f);
                    Vector3 bottomLeftCorner = new Vector3 (boxPos.x - wHalf, boxPos.y - hHalf, 1f);
                    Vector3 bottomRightCorner = new Vector3 (boxPos.x + wHalf, boxPos.y - hHalf, 1f);
                    Gizmos.DrawLine (topLeftCorner, topRightCorner);
                    Gizmos.DrawLine (topRightCorner, bottomRightCorner);
                    Gizmos.DrawLine (bottomRightCorner, bottomLeftCorner);
                    Gizmos.DrawLine (bottomLeftCorner, topLeftCorner);
                }
            }
        }
        else if (waveData.spawnLocationType == SpawnLocation.SpawnPoint)
        {
            if (waveData.spawns.Length != 0)
            {
                for (int i = 0; i < waveData.spawns.Length; i++)
                {
                    Vector3 spawnPos = new Vector3(waveData.spawns[i].position.x, waveData.spawns[i].position.y, 1f);

                    #if UNITY_EDITOR
                    UnityEditor.Handles.DrawWireDisc(spawnPos ,Vector3.back, 0.15f);
                    UnityEditor.Handles.DrawWireDisc(spawnPos ,Vector3.back, 0.25f);
                    #endif

                }
            }

            // Gizmos.color = Color.yellow;
            // float wHalf = (waveData.spawnBoxWidth * .5f);
            // float hHalf = (waveData.spawnBoxHeight * .5f);
            // Vector3 topLeftCorner = new Vector3 (waveData.spawnBoxPosX - wHalf, waveData.spawnBoxPosY + hHalf, 1f);
            // Vector3 topRightCorner = new Vector3 (waveData.spawnBoxPosX + wHalf, waveData.spawnBoxPosY + hHalf, 1f);
            // Vector3 bottomLeftCorner = new Vector3 (waveData.spawnBoxPosX - wHalf, waveData.spawnBoxPosY - hHalf, 1f);
            // Vector3 bottomRightCorner = new Vector3 (waveData.spawnBoxPosX + wHalf, waveData.spawnBoxPosY - hHalf, 1f);
            // Gizmos.DrawLine (topLeftCorner, topRightCorner);
            // Gizmos.DrawLine (topRightCorner, bottomRightCorner);
            // Gizmos.DrawLine (bottomRightCorner, bottomLeftCorner);
            // Gizmos.DrawLine (bottomLeftCorner, topLeftCorner);
        }
     }

    // Instantiates the list of enemy prefabs from the Wave Scriptable Object
    private List<List<GameObject>> InstantiateEnemies(GameObject[] enemyPrefabList)
    {
        List<List<GameObject>> spawnEnemiesList = new List<List<GameObject>>();
        GameObject enemy;

        if (waveData.spawns != null)
        {
            for (int i = 0; i < waveData.spawns.Length; i++)
            {
                List<GameObject> enemyList = new List<GameObject>();

                for(int j = 0; j < enemyPrefabList.Length; j++)
                {
                    enemy = Instantiate(enemyPrefabList[j]);
                    enemy.transform.parent = this.transform;
                    enemy.GetComponent<Enemy>().InitializeEnemy();
                    enemy.SetActive(false);
                    enemyList.Add(enemy);
                }

                spawnEnemiesList.Add(enemyList);
            }
        }

        return spawnEnemiesList;
    }
    private void InstantiateSpawnPortals()
    {
        for (int i = 0; i < spawnLocations.Length; i++)
        {
            GameObject spawnPortalObj;
            Vector3 spawnLocationPos;

            spawnLocationPos = new Vector3(spawnLocations[i].position.x, spawnLocations[i].position.y, 0);

            spawnPortalObj = Instantiate(spawnPortalPrefab, spawnLocationPos, Quaternion.identity);
            spawnPortalObj.SetActive(false);

            spawnPortals.Add(spawnPortalObj);
        }
    }


    IEnumerator SpawnWave()
    {
        if (enemies.Count > 0)
        {
            // waveData.enemies.Length represents the number of enemies in a single List of spawn locations
            for (int i = 0; i < waveData.enemies.Length; i++)       // Probably bad to reference the waveData directly here. Maybe change to a private class variable instead
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    Debug.Log("Spawning Enemy in SpawnWave method!");

                    if (enemySpawnOrder == SpawnOrder.Simultaneous)
                    {
                        if (spawnLocationType == SpawnLocation.SpawnBox)
                        {
                            // float leftBound = spawnBoxPosition.x - spawnBoxDimensions.x/2f;
                            // float rightBound = spawnBoxPosition.x + spawnBoxDimensions.x/2f;
                            // float bottomBound = spawnBoxPosition.y - spawnBoxDimensions.y/2f;
                            // float topBound = spawnBoxPosition.y + spawnBoxDimensions.y/2f;
                            float leftBound = spawnLocations[j].position.x - spawnLocations[j].width/2f;
                            float rightBound = spawnLocations[j].position.x + spawnLocations[j].width/2f;
                            float bottomBound = spawnLocations[j].position.y - spawnLocations[j].height/2f;
                            float topBound = spawnLocations[j].position.y + spawnLocations[j].height/2f;

                            Vector2 spawnPos = new Vector2(Random.Range(leftBound, rightBound), Random.Range(bottomBound, topBound));
                            enemies[j][i].GetComponent<Enemy>().SpawnEnemy(spawnPos);
                            enemies[j][i].SetActive(true);
                        }
                        else
                            enemies[j][i].GetComponent<Enemy>().SpawnEnemy();
                    }
                }

                // successiveUnitDelay is set in seconds, so multiplying by 1000 for milliseconds.
                if (i < waveData.enemies.Length - 1)
                    yield return new WaitForSeconds(successiveUnitDelay);
            }

            foreach (GameObject spawnPort in spawnPortals)
                spawnPort.GetComponent<SpawnPortal>().EndSpawn();
        }

        CurrentWaveState = WaveState.WaveSpawnComplete;
        WaveStart?.Invoke();
            
    }


    private bool IsWaveOver()
    {
        foreach (List<GameObject> spawnList in enemies)
        {
            foreach (GameObject e in spawnList)
            {
                if (e.GetComponent<Enemy>().IsAlive)
                    return false;
            }
        }

        activeWaveSet.Remove(this);

        return true;
    }

    // Method used by the 'Level' class to initiate the wave
    // This may actually not be necessary if I just keep the GameObject inactive.
    public void InitiateWave()
    {
        CurrentWaveState = WaveState.Initiated;
    }

    // Used by the 'Level' class to easily check when the wave is complete. 
    public bool IsWaveComplete()
    {
        return CurrentWaveState == WaveState.WaveComplete;
    }

    public void CompleteWave()
    {
        foreach(GameObject enemy in spawnEnemies)
            enemy.GetComponent<Enemy>().DestroyEnemy();

        WaveComplete?.Invoke();
    }


    // Used when waveSpawnType == SpawnType.Trigger
    public void TriggerWaveSpawn()
    {
        waveTrigger = true;
    }
}
