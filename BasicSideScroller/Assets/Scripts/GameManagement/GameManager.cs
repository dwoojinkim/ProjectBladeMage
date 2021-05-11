using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject GameTimerObj;
    public GameObject Minion;
    public GameObject Box;
    public GameObject Log;

    public Vector3 EnemySpawn = new Vector3(0,0,0);

    public int InitialEnemySpeed = 2;
    public int EnemyAcceleration = 2;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private List<GameObject> inactiveEnemies = new List<GameObject>();

    private float gameTime = 0.0f;
    private int enemySpeed;
    private float spawnTimer = 0.0f;
    private float timeToSpawn = 2.0f;

    private Text GameTimerText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
 
            //Initialization code goes here[/INDENT]
            GameObject enemy;
            enemySpeed = InitialEnemySpeed;
            GameTimerText = GameTimerObj.GetComponent<Text>();

            //Create a pool of enemies
            enemy = Instantiate(Minion, EnemySpawn, Quaternion.identity);
            inactiveEnemies.Add(enemy);
            enemy = Instantiate(Box, EnemySpawn, Quaternion.identity);
            inactiveEnemies.Add(enemy);
            enemy = Instantiate(Log, EnemySpawn, Quaternion.identity);
            inactiveEnemies.Add(enemy);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
        GameTimerText.text = gameTime.ToString();

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= timeToSpawn)
        {
            SpawnEnemy();
            spawnTimer = 0.0f;
            Debug.Log("Spawned Enemy!");
        }
    }

    private void FixedUpdate()
    {

        //Move all enemies to the left a certain speed
        /*
        foreach (GameObject enemy in activeEnemies)
        {
            enemy.transform.position -= transform.right * Time.fixedDeltaTime * enemySpeed;

            if (enemy.transform.position.x < -10)
                ResetEnemy(enemy);
        }*/

        for (int i = 0; i < activeEnemies.Count; i++)
        {
            activeEnemies[i].transform.position -= transform.right * Time.fixedDeltaTime * enemySpeed;

            if (activeEnemies[i].transform.position.x < -10)
                ResetEnemy(activeEnemies[i]);
        }
    }

    public void ResetGame()
    {
        gameTime = 0f;
    }

    private void SpawnEnemy()
    {
        GameObject enemy;

        enemy = inactiveEnemies[0];
        enemy.GetComponent<Obstacle>().Spawn();
        inactiveEnemies.RemoveAt(0);
        activeEnemies.Add(enemy);
    }

    private void ResetEnemy(GameObject enemy)
    {
        GameObject resetEnemy;

        enemy.transform.position = EnemySpawn;

        enemy.GetComponent<Obstacle>().Kill();
        resetEnemy = activeEnemies[0];
        activeEnemies.RemoveAt(0);
        inactiveEnemies.Add(resetEnemy);
    }
}
