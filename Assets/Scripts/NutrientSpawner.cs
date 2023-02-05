using System;
using Roots;
using UnityEngine;

public class NutrientSpawner : MonoBehaviour
{
    public GameObject nutrient;
    public int totalNutrientsToSpawn;
    private Spawner _spawner;
    private bool _respawn = false;
    private int _nutrientCount;

    public void SpawnNutrientsButton()
    {
        _spawner.DestroyObjectsIfExists();
        _spawner.RandomlySpawnObjects(nutrient, totalNutrientsToSpawn, "Nutrient");
    }

    bool ShouldRespawnNutrients()
    {
        Debug.Log($"checking if nutrients should respond: result = {_respawn}");
        if (_nutrientCount < totalNutrientsToSpawn / 2 && !_respawn) {
            _respawn = true;
        }
        return _respawn;
    }

    void RespawnNutrientsOverTime()
    {
        int respawnInterval = 1;
        float timeSinceLastRespawn = 1f; 
        timeSinceLastRespawn += Time.deltaTime;
        
        if (timeSinceLastRespawn >= respawnInterval)
        {
            timeSinceLastRespawn = 0;
            Debug.Log("Spawning more nutrients");
            _spawner.RandomlySpawnObjects(nutrient, 1, "Nutrient");
            
            if (_nutrientCount >= totalNutrientsToSpawn) _respawn = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _spawner = gameObject.AddComponent<Spawner>();
        _spawner.RandomlySpawnObjects(nutrient, totalNutrientsToSpawn, "Nutrient");
    }

    private void Update()
    {
        _nutrientCount = GameObject.FindGameObjectsWithTag("Nutrient").Length;
        if (ShouldRespawnNutrients())
        {
            RespawnNutrientsOverTime();
        }
    }
}
