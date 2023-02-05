using UnityEngine;
using UnityEngine.Serialization;

namespace Flower
{
    public class RootNutrientReserve : Helpers.Singleton<RootNutrientReserve>
    {
        public float NutrientsInReserve = 10;
        public float DistanceCostMultiplier = 0.5f;

        [ContextMenu("Add Nutrients")]
        public void DebugAddNutrient()
        {
            AddNutrient(100);
        }

        public void AddNutrient(float nutrientAmount)
        {
            NutrientsInReserve += nutrientAmount;
        }

        public void SubtractNutrient(float nutrientAmount)
        {
            NutrientsInReserve -= nutrientAmount;
        }
        
        public void SubtractNutrientByDistance(float nutrientAmount)
        {
            NutrientsInReserve -= (nutrientAmount * DistanceCostMultiplier);
        }
    }
}
