using UnityEngine;
using UnityEngine.Serialization;

namespace Flower
{
    public class RootNutrientReserve : Helpers.Singleton<RootNutrientReserve>
    {
        public GameObject deathScreen;
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
            if (NutrientsInReserve <= 0)
            {
                Death();
            }
        }

        private void Death()
        {
            deathScreen.SetActive(true);
        }
        
        public void SubtractNutrientByDistance(float nutrientAmount)
        {
            SubtractNutrient(nutrientAmount * DistanceCostMultiplier);
        }
    }
}
