using Flower;
using UnityEngine;

namespace DefaultNamespace
{
    public class Nutrient : MonoBehaviour, IRootCollision
    {
        public float Value;
        public bool HandleRootPartCollision(RootPart rootPart, Collider collidingPart)
        {
            var currentNutrients = RootNutrientReserve.Instance;
            currentNutrients.AddNutrient(Value);
            Debug.Log($"Found Nutrient! value: {Value}. Nutrient Reserve is {currentNutrients.NutrientsInReserve}");
            PlaySoundEffect.Instance.PlayNutrientPickUpSound();
            Destroy(gameObject, 0.01f);
            return false; // false means do not destroy root part
        }
    }
}