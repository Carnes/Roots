using UnityEngine;

namespace Flower
{
    public class RootNutrientReserve : Helpers.Singleton<RootNutrientReserve>
    {
        public float NutrientsInReserve { get; private set; } = 10;

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
    }
}
