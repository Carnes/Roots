using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public Vector3 minBounds = new Vector3(-25f, -28f);
    public Vector3 maxBounds = new Vector3(21f, -4f);
    public float z = 81f;
    private GameObject[] _existingObjects = Array.Empty<GameObject>();

    void DestroyObjectsIfExists()
    {
        if (_existingObjects.Length > 0)
        {
            foreach (GameObject existingObjects in _existingObjects)
            {
                Destroy(existingObjects);
            }
        }
    }
    private bool IsTooClose(List<Vector3> objectSpawnPoints, Vector3 randomPosition)
    {
        float distanceThreshold = 5f;
        foreach (Vector3 spawnPoint in objectSpawnPoints)
        {
            float distance = Vector3.Distance(randomPosition, spawnPoint);
            if (distance < distanceThreshold) return true;
        }
        return false;
    }
    
    public void RandomlySpawnObjects(GameObject objectToSpawn, int numObjectsToSpawn, string objectTypeToSpawn)
    {
        List<Vector3> spawnPoints = new List<Vector3>();
        for (int i = 0; i < numObjectsToSpawn; i++)
        {
            float x = Mathf.Clamp(Random.Range(minBounds.x, maxBounds.x), minBounds.x, maxBounds.x);
            float y = Mathf.Clamp(Random.Range(minBounds.y, maxBounds.y), minBounds.y, maxBounds.y);

            Vector3 randomPosition = new Vector3(x, y, z);

            if (!IsTooClose(spawnPoints, randomPosition))
            {
                spawnPoints.Add(randomPosition);

                var gameObjectToSpawn = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
                if (objectTypeToSpawn == "Nutrient")
                {
                    var newNutrient = gameObjectToSpawn.GetComponent<Nutrient>();
                    newNutrient.Value = Random.Range(1,4);

                    float scalingFactor = newNutrient.Value / 2;
                    gameObjectToSpawn.transform.localScale = new Vector3(scalingFactor, scalingFactor, scalingFactor);
                }

                if (objectTypeToSpawn == "Obstacle")
                {
                    gameObjectToSpawn.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                    gameObjectToSpawn.transform.localScale = new Vector3(Random.Range(1f, 2f), Random.Range(1f, 2f), 1);
                }
            }
            else i--;
        }
        _existingObjects = GameObject.FindGameObjectsWithTag(objectTypeToSpawn);
    }
    
}
