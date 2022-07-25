using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Notify();  // delegate

public class Wave : MonoBehaviour
{
    public enum WaveState
    {
        Standby,
        Initiated,
        WaveStarted,
        WaveComplete
    };

    public WaveState CurrentWaveState {get; private set;}
    public event Notify WaveStart;      // event

    [SerializeField] private WaveSO waveData;
    [SerializeField] private WaveRuntimeSet activeWaveSet;

    private SpawnType waveSpawnType;
    private SpawnLocation spawnLocationType;
    private EnemyWaveType enemyWaveType;

    private List<GameObject> enemies = new List<GameObject>();
    private Vector2 spawnBoxDimensions;
    private Vector2 spawnBoxPosition;

    private float timer;
    private float timeUntilSpawn;
    private float successiveUnitDelay;
 
 
    // Start is called before the first frame update
    void Awake()
    {
        CurrentWaveState = WaveState.Standby;

        waveSpawnType = waveData.waveSpawnType;
        spawnLocationType = waveData.spawnLocationType;
        enemyWaveType = waveData.enemyWaveType;
        
        timer = 0;
        timeUntilSpawn = waveData.TimeUntilSpawn;
        successiveUnitDelay = waveData.SuccessiveUnitDelay;

        spawnBoxDimensions.x = waveData.spawnBoxWidth;
        spawnBoxDimensions.y = waveData.spawnBoxHeight;

        spawnBoxPosition.x = waveData.spawnBoxPosX;
        spawnBoxPosition.y = waveData.spawnBoxPosY;

        enemies = InstantiateEnemies(waveData.enemies);
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentWaveState == WaveState.Initiated) // || waveState == WaveState.WaveComplete) // It should only check WaveComplete state to restart if it's supposed to re-spawn
        {
            timer += Time.deltaTime;
            if (timer > timeUntilSpawn)
            {
                Debug.Log("Spawning Wave!");
                CurrentWaveState = WaveState.WaveStarted;
                StartCoroutine(SpawnWave());
            }
        }

        if (CurrentWaveState == WaveState.WaveStarted && IsWaveOver())
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
        if (spawnLocationType == SpawnLocation.SpawnBox)
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
    
    IEnumerator SpawnWave()
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

                // successiveUnitDelay is set in seconds, so multiplying by 1000 for milliseconds.
                yield return new WaitForSeconds(successiveUnitDelay);
            }

            WaveStart?.Invoke();
        }
    }

    private bool IsWaveOver()
    {
        foreach (GameObject e in enemies)
        {
            if (e.GetComponent<Enemy>().IsAlive)
                return false;
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

}
