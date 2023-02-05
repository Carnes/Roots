using UnityEngine;

public class NutrientSpawner : MonoBehaviour
{
    public GameObject nutrient;
    public int totalNutrientsToSpawn;
    private Spawner _spawner;

    public void SpawnNutrientsButton()
    {
        _spawner.DestroyObjectsIfExists();
        _spawner.RandomlySpawnObjects(nutrient, totalNutrientsToSpawn, "Nutrient");
    }

    // Start is called before the first frame update
    void Start()
    {
        _spawner = gameObject.AddComponent<Spawner>();
        _spawner.RandomlySpawnObjects(nutrient, totalNutrientsToSpawn, "Nutrient");
    }
}
