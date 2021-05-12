﻿using System.Collections;
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
    private float timeToSpawn = 2.0f;
    private float respawnDistance = 26.0f;

    void Start()
    {
        ObstacleSpawnPos = transform.position;
        GameObject obstacle;

        //Create a pool of enemies
        obstacle = Instantiate(Minion, ObstacleSpawnPos, Quaternion.identity);
        inactiveObstacles.Add(obstacle);
        obstacle = Instantiate(Box, ObstacleSpawnPos, Quaternion.identity);
        inactiveObstacles.Add(obstacle);
        obstacle = Instantiate(Log, ObstacleSpawnPos, Quaternion.identity);
        inactiveObstacles.Add(obstacle);
    }

    // Update is called once per frame
    void Update()
    {
        ObstacleSpawnPos = transform.position;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= timeToSpawn)
        {
            SpawnObstacle();
            spawnTimer = 0.0f;
        }
    }

    private void FixedUpdate()
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

        obstacle = inactiveObstacles[0];
        obstacle.transform.position = ObstacleSpawnPos;
        obstacle.GetComponent<Obstacle>().Spawn();
        inactiveObstacles.RemoveAt(0);
        activeObstacles.Add(obstacle);
    }

    private void ResetObstacle(GameObject obstacle)
    {
        GameObject resetObstacle;

        obstacle.GetComponent<Obstacle>().Kill();
        resetObstacle = activeObstacles[0];
        activeObstacles.RemoveAt(0);
        inactiveObstacles.Add(resetObstacle);
    }
}
