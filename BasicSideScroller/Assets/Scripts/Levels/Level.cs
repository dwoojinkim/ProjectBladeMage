using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private enum LevelState
    {
        Uninitiated,
        LevelStarted,
        LevelComplete
    };

    public event Notify LevelComplete;      // event

    [SerializeField] private LevelSO levelData;
    [SerializeField] private GameObject nextLevelPortals;
    [SerializeField] private Wave[] waves;  // I probably can get rid of this once I get the LevelSO data working.
    [SerializeField] private GameObjectReference currentStage;
    [SerializeField] private FloatVariable currentStageSpeed;


    private string levelName;
    //private List<GameObject> waves;   // Uncomment once I finish integrating ScriptableObject data.
    private LevelState levelState;
    private int currentWave;

    // Start is called before the first frame update
    void Start()
    {
        //if (levelData == null) throw new System.ArgumentNullException("levelData is null");   // Uncomment once I finish integrating ScriptableObject data.

        //levelName = levelData.levelName;  // Uncomment once I finish integrating ScriptableObject data.

        levelState = LevelState.LevelStarted;   // Level will actually be started by LevelManager. For testing, it'll be initialzed as started.
        currentWave = -1;                        // currentWave should start at -1 and be set to 0 when started by the LevelManager.    
        currentStage.SetGameObject(this.gameObject);

        // subscribe to WaveStart event. Used for multiple Timer waves in a row. Starts timer on previous wave's spawn.
        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].WaveStart += InitiateNextWave;
            Debug.Log("Subcribing Wave Event");
            waves[i].gameObject.SetActive(false);
        }

        InitiateNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelState == LevelState.LevelStarted)
        {
            if (IsLevelComplete())
            {
                CompleteLevel();
            }
        }
    }

    // 'Wave' class also uses the same method. Maybe it can eventually be moved to a static
    // class or something.
    private List<GameObject> InstantiateWaves(GameObject[] wavePrefabList)
    {
        List<GameObject> waveList = new List<GameObject>();
        GameObject wave;

        for(int i = 0; i < wavePrefabList.Length; i++)
        {
            wave = Instantiate(wavePrefabList[i]);
            wave.SetActive(false);
            waveList.Add(wave);
        }

        return waveList;
    }

    private bool IsLevelComplete()
    {   
        foreach(Wave wave in waves)
        {
            if (!wave.IsWaveComplete())
                return false;
        }

        return true;
    }

    private void CompleteLevel()
    {
        levelState = LevelState.LevelComplete;
        Debug.Log("LEVEL COMPLETE!");
        
        foreach (Wave wave in waves)
            wave.CompleteWave();

        LevelComplete?.Invoke();
        nextLevelPortals.transform.position = new Vector3(0, 0, 0);
        //Destroy(this.gameObject);
    }

    private void InitiateNextWave()
    {
        if (currentWave < waves.Length && currentWave > 0)
            waves[currentWave].WaveStart -= InitiateNextWave;

        currentWave++;

        if (currentWave < waves.Length) // Change to waves.Count when I make it a List rather than an Array.
        {
            waves[currentWave].gameObject.SetActive(true);
            waves[currentWave].InitiateWave();
        }
    }

    private void InitiateSubsequentWave()
    {
        waves[currentWave].WaveStart -= InitiateSubsequentWave;

        Debug.Log("Starting  subsequent wave!");
        waves[currentWave+1].InitiateWave();
    }

    public void StartLevel()
    {
        levelState = LevelState.LevelStarted;
        InitiateNextWave();
    }
}
