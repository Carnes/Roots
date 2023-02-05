using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Flower
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class RootPart : MonoBehaviour, IGrowPoint
    {
        public Vector3 Start;
        public Vector3 End;
        public bool HasRoomForGrowth;
        public Root ChildRoot;

        public Vector3 GrowPosition => transform.position + new Vector3(0,0,-0.25f);

        private Root Root => transform.parent.GetComponent<Root>();

        public bool GrowToWorldPoint(Vector3 worldPoint)
        {
            
            var newRoot = Root.AddChildRoot(transform.position, worldPoint);
            ChildRoot = newRoot;
            HasRoomForGrowth = ChildRoot != null;
            return !HasRoomForGrowth;
        }

        public void Set(Vector3 start, Vector3 end, bool hasRoomForGrowth)
        {
            HasRoomForGrowth = hasRoomForGrowth;
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