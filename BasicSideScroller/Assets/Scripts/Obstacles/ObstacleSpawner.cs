using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject Minion;
    public GameObject Box;
    public GameObject Log;

    private Vector3 ObstacleSpawnPos;

    private List<GameObject> activeObstacles = new List<GameObject>();
    private List<GameObject> inactiveObstacles = new List<GameObject>();

    private float spawnTimer = 0.0f;
    private float timeToSpawn = 1.0f;
    private float respawnDistance = 30.0f;
    void Start()
    {
        ObstacleSpawnPos = transform.position;
        //GameObject obstacle;

        //Create a pool of enemies
/*         obstacle = Instantiate(Minion, ObstacleSpawnPos, Quaternion.identity);
        inactiveObstacles.Add(obstacle);
        obstacle = Instantiate(Box, ObstacleSpawnPos, Quaternion.identity);
        inactiveObstacles.Add(obstacle);
        obstacle = Instantiate(Log, ObstacleSpawnPos, Quaternion.identity);
        inactiveObstacles.Add(obstacle); */
        // New pool code
        CreateObstaclePool(ref inactiveObstacles, Minion, 3);
        CreateObstaclePool(ref inactiveObstacles, Box, 3);
        CreateObstaclePool(ref inactiveObstacles, Log, 3);
    }

    // Update is called once per frame
    void Update()
    {
        ObstacleSpawnPos = transform.position;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= timeToSpawn && inactiveObstacles.Count > 0)
        {
            SpawnObstacle();
            spawnTimer = 0.0f;
        }
    }

    private void FixedUpdate()
    {

    }

    void LateUpdate()
    {
        for (int i = 0; i < activeObstacles.Count; i++)
        {
            if (Mathf.Abs(activeObstacles[i].transform.position.x - ObstacleSpawnPos.x) > respawnDistance)
                ResetObstacle(activeObstacles[i]);
        }
    }

    private void SpawnObstacle()
    {
        GameObject obstacle;
        int randomObstacleIndex = Random.Range(0, inactiveObstacles.Count);

        obstacle= inactiveObstacles[randomObstacleIndex];
        obstacle.transform.position = ObstacleSpawnPos;
        obstacle.GetComponent<Obstacle>().Spawn();
        inactiveObstacles.RemoveAt(randomObstacleIndex);
        activeObstacles.Add(obstacle);
    }

        private void ResetObstacle(GameObject obstacle)
    {
        obstacle.GetComponent<Obstacle>().Kill();
        obstacle = activeObstacles[0];
        activeObstacles.RemoveAt(0);
        inactiveObstacles.Add(obstacle);
    }

    private void CreateObstaclePool(ref List<GameObject> pool, GameObject obj, int numObj)
    {
        GameObject instantiatedObj;
        for (int i = 0; i < numObj; i++)
        {
            instantiatedObj = Instantiate(obj, Vector3.zero, Quaternion.identity);
            pool.Add(instantiatedObj);
        }
    }


    //Test function if this is a way to move all gameobjects back seamlessly and prevent
    //players from eventually getting to too high have an X position.
    public void MoveObstaclesBack(float resetDistance)
    {
        for (int i = 0; i < activeObstacles.Count; i++)
            activeObstacles[i].transform.position -= transform.right * resetDistance;
    }

    public void ResetSpawner()
    {
        int activeObstaclesCount = activeObstacles.Count;
        // Reset all currently active obstacles
        for (int i = 0; i < activeObstaclesCount; i++)
        {
            // Since ResetObstacle keeps removing the first index of activeObstacles,
            // The 0th index will always have to be reset.
            ResetObstacle(activeObstacles[0]);
        }

        spawnTimer = 0.0f;
    }
}
