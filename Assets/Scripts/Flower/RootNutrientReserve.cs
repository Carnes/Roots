using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Flower
{
    public class RootNutrientReserve : Helpers.Singleton<RootNutrientReserve>
    {
        public GameObject deathScreen;
        public GameObject victoryScreen;
        public float NutrientsInReserve = 10;
        public float DistanceCostMultiplier = 0.5f;

        public override void SingletonStart()
        {
            Alive();
        }

        [ContextMenu("Add Nutrients")]
        public void DebugAddNutrient()
        {
            AddNutrient(50);
        }

        public void AddNutrient(float nutrientAmount)
        {
            NutrientsInReserve += nutrientAmount;

            if (NutrientsInReserve >= 50)
            {
                Victory();
            }

            if (NutrientsInReserve > 0f)
                Alive();
        }

        public void SubtractNutrient(float nutrientAmount)
        {
            NutrientsInReserve -= nutrientAmount;
            if (NutrientsInReserve <= 0)
            {
                Death();
            }
        }

        private void Victory()
        {
            victoryScreen.SetActive(true);
        }

        private void Death()
        {
            deathScreen.SetActive(true);
        }
        
        private void Alive()
        {
            deathScreen.SetActive(false);
        }
        
        public void SubtractNutrientByDistance(float nutrientAmount)
        {
            SubtractNutrient(nutrientAmount * DistanceCostMultiplier);
        }
    }
}
