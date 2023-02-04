using System.Collections.Generic;
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

        private Root Root => transform.parent.GetComponent<Root>();

        public void Set(Vector3 start, Vector3 end)
        {
            Start = start;
            End = end;
            SetTransform();
            SetCollider();
        }

        private void SetTransform()
        {
            var direction = (End - Start).normalized;
            var dist = Vector3.Distance(Start, End);
            var middlePoint = ((dist / 2) * direction) + Start;

            transform.position = Vector3.zero;
            transform.localPosition = middlePoint;
            transform.forward = direction;
        }

        private void SetCollider()
        {
            var dist = Vector3.Distance(Start, End);
            var collider = GetComponent<CapsuleCollider>();
            collider.height = dist;
            collider.isTrigger = true;
        }

        protected void OnTriggerEnter(Collider other)
        {
            var collisionHandler = other.gameObject.GetComponent<IRootCollision>();
            if (collisionHandler != null)
            {
                var shouldDestroyRoots = collisionHandler.HandleRootPartCollision(this, other);
                if (shouldDestroyRoots)
                    Root.RootPartHit(this);
            }
        }
    }
}