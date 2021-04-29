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

    private List<GameObject> enemies = new List<GameObject>();

    private int enemySpeed;
    // Start is called before the first frame update
    void Start()
    {
        GameObject enemy;
        enemySpeed = InitialEnemySpeed;

        //Create a pool of enemies
        enemy = Instantiate(Minion, EnemySpawn, Quaternion.identity);
        enemies.Add(enemy);
        enemy = Instantiate(Box, EnemySpawn, Quaternion.identity);
        enemies.Add(enemy);
        enemy = Instantiate(Log, EnemySpawn, Quaternion.identity);
        enemies.Add(enemy);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        //Move all enemies to the left a certain speed
        foreach (GameObject enemy in enemies)
        {
            enemy.transform.position -= transform.right * Time.fixedDeltaTime * enemySpeed;

            if (enemy.transform.position.x < -10)
                ResetEnemy(enemy);
            //Debug.Log("Enemy Position: " + enemy.transform.position.x);
        }
    }

    private void ResetEnemy(GameObject enemy)
    {
        enemy.transform.position = EnemySpawn;
    }
}
