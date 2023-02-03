using DefaultNamespace;
using UnityEngine;

namespace Flower
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class RootPart : MonoBehaviour
    {
        public void Set(Vector3 start, Vector3 end)
        {
            var direction = (end - start).normalized;
            var dist = Vector3.Distance(start, end);
            var middlePoint = ((dist / 2) * direction) + start;

            transform.position = Vector3.zero;
            transform.localPosition = middlePoint;
            transform.forward = direction;

            var collider = GetComponent<CapsuleCollider>();
            collider.height = dist;
            collider.isTrigger = true;
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Nutrient"))
            {
                var nutrient = other.gameObject.GetComponent<Nutrient>();
                Debug.Log($"Found Nutrient! value: {nutrient.Value}");
                Destroy(other.gameObject);
            }
            else
            {
                Debug.Log($"Collision: {other.gameObject.name} hit {gameObject.name}");
            }
        }
    }
}