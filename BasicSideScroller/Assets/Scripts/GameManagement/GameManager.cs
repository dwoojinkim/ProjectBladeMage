using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        GameObject enemy;
        enemySpeed = InitialEnemySpeed;

        //Create a pool of enemies
        enemy = Instantiate(Minion, EnemySpawn, Quaternion.identity);
        inactiveEnemies.Add(enemy);
        enemy = Instantiate(Box, EnemySpawn, Quaternion.identity);
        inactiveEnemies.Add(enemy);
        enemy = Instantiate(Log, EnemySpawn, Quaternion.identity);
        inactiveEnemies.Add(enemy);
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
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

    private void SpawnEnemy()
    {
        GameObject enemy;

        enemy = inactiveEnemies[0];
        inactiveEnemies.RemoveAt(0);
        activeEnemies.Add(enemy);
    }

    private void ResetEnemy(GameObject enemy)
    {
        GameObject resetEnemy;

        enemy.transform.position = EnemySpawn;

        resetEnemy = activeEnemies[0];
        activeEnemies.RemoveAt(0);
        inactiveEnemies.Add(resetEnemy);
    }
}
