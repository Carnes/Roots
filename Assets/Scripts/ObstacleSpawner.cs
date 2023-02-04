using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject obstacle2;
    public GameObject obstacle3;
    
    public int totalObstaclesToSpawn = 6;

    private Spawner _spawner;
    // Start is called before the first frame update
    GameObject GetRandomObstacle()
    {
        var obstacles = new List<GameObject>();
        {
            obstacles.Add(obstacle);
            obstacles.Add(obstacle2);
            obstacles.Add(obstacle3);
        }
        int randomIndex = Random.Range(0, obstacles.Count);
        GameObject randomObstacle = obstacles[randomIndex];

        return obstacles[randomIndex];
    }
    void Start()
    {
        _spawner = gameObject.AddComponent<Spawner>();
        for (int i = 0; i < totalObstaclesToSpawn; i++)
        {
            _spawner.RandomlySpawnObjects(GetRandomObstacle(), 1, "Obstacle");
        }
    }
}
