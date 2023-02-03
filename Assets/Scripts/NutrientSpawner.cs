using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class NutrientSpawner : MonoBehaviour
{
    public GameObject nutrient;
    public int totalNutrientsToSpawn = 10;
    public Vector3 minBounds = new Vector3(-11f, -28f);
    public Vector3 maxBounds = new Vector3(6f, -4f);
    public float z = 81f;
    public float NutrientValueMin = 2;
    public float NutrientValueMax = 4;
    private GameObject[] _existingNutrients = Array.Empty<GameObject>();

    void DestroyNutrientsIfExists()
    {
        if (_existingNutrients.Length > 0)
        {
            foreach (GameObject existingNutrient in _existingNutrients)
            {
                Destroy(existingNutrient);
            }
        }
    }
    
    private bool IsTooClose(List<Vector3> nutrientSpawnPoints, Vector3 randomPosition)
    {
        float distanceThreshold = 5f;
        foreach (Vector3 spawnPoint in nutrientSpawnPoints)
        {
            float distance = Vector3.Distance(randomPosition, spawnPoint);
            if (distance < distanceThreshold) return true;
        }
        return false;
    }
    
    public void RandomlySpawnNutrients()
    {
        DestroyNutrientsIfExists();
    
        List<Vector3> nutrientSpawnPoints = new List<Vector3>();
        for (int i = 0; i < totalNutrientsToSpawn; i++)
        {
            float x = Mathf.Clamp(Random.Range(minBounds.x, maxBounds.x), minBounds.x, maxBounds.x);
            float y = Mathf.Clamp(Random.Range(minBounds.y, maxBounds.y), minBounds.y, maxBounds.y);

            Vector3 randomPosition = new Vector3(x, y, z);

            if (!IsTooClose(nutrientSpawnPoints, randomPosition))
            {
                nutrientSpawnPoints.Add(randomPosition);
                var newNutrientGameObject = Instantiate(nutrient, randomPosition, Quaternion.identity);
                var newNutrient = newNutrientGameObject.GetComponent<Nutrient>();
                newNutrient.Value = Random.Range(NutrientValueMin, NutrientValueMax);
            }
            else i--;
        }
        _existingNutrients = GameObject.FindGameObjectsWithTag("Nutrient");
    }
    // Start is called before the first frame update
    void Start()
    {
        RandomlySpawnNutrients();
    }
}
