using UnityEngine;

public class NutrientSpawner : MonoBehaviour
{
    public GameObject nutrient;
    public int totalNutrientsToSpawn = 15;
    private Spawner _spawner;

    // Start is called before the first frame update
    void Start()
    {
        _spawner = gameObject.AddComponent<Spawner>();
        _spawner.RandomlySpawnObjects(nutrient, totalNutrientsToSpawn, "Nutrient");
    }
}
