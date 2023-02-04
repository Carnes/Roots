using DefaultNamespace;
using UnityEngine;

namespace Flower
{
    public class RootNutrientReserve : MonoBehaviour
    {
        //Set up singleton pattern for nutrient reserve so it can be used as the same bank for all individual root parts
        //otherwise it would not increment and instead reset to only adding the value of the current nutrient being collected to the reserve
        private static RootNutrientReserve _instance;
        public static RootNutrientReserve Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<RootNutrientReserve>();
                    if (!_instance)
                    {
                        var go = new GameObject("RootNutrientReserve");
                        _instance = go.AddComponent<RootNutrientReserve>();
                    }
                }
                return _instance;
            }
        }
        public float NutrientsInReserve { get; private set; } = 10;

        public void AddNutrient(float nutrientAmount)
        {
            NutrientsInReserve += nutrientAmount;
        }

        public void SubtractNutrient(float nutrientAmount)
        {
            NutrientsInReserve -= nutrientAmount;
        }

    }
}
