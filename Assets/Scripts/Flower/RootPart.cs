using DefaultNamespace;
using UnityEngine;

namespace Flower
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class RootPart : MonoBehaviour
    {
        public Vector3 Start;
        public Vector3 End;
        public RootNutrientReserve currentNutrients;
        
        public void Set(Vector3 start, Vector3 end)
        {
            Start = start;
            End = end;
            var direction = (end - start).normalized;
            var dist = Vector3.Distance(start, end);
            var middlePoint = ((dist / 2) * direction) + start;

            transform.position = Vector3.zero;
            transform.localPosition = middlePoint;
            transform.forward = direction;

            var collider = GetComponent<CapsuleCollider>();
            collider.height = dist;
            collider.isTrigger = true;
            
            currentNutrients = RootNutrientReserve.Instance;
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Nutrient"))
            {
                var nutrient = other.gameObject.GetComponent<Nutrient>();
                currentNutrients.AddNutrient(nutrient.Value);
                Debug.Log($"Found Nutrient! value: {nutrient.Value}. Nutrient Reserve is {currentNutrients.NutrientsInReserve}");
                Destroy(other.gameObject);
            }
            else if (other.gameObject.CompareTag("Spider"))
            {
                Debug.Log($"Spider strike!");
                transform.parent.GetComponent<Root>().RootPartHit(this);
            }
            else
            {
                Debug.Log($"Collision: {other.gameObject.name} hit {gameObject.name}");
            }
        }
    }
}