using System;
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
        public bool IsGrowing = true;
        public float GrowSpeed = 0.5f;
        
        private CapsuleCollider _collider;
        private float _growthPercentage = 0f;

        public Vector3 GrowPosition => transform.position + new Vector3(0,0,-0.25f);

        private Root Root => transform.parent.GetComponent<Root>();
        
        public Vector3 CurrentEnd => CalcCurrentEnd();
        
        private Vector3 CalcCurrentEnd()
        {
            if (IsGrowing)
            {
                var direction = (End - Start).normalized;

                var fullDist = Vector3.Distance(Start, End);
                var currentDist = Mathf.Lerp(0f, fullDist, _growthPercentage);
                var currentEnd = (direction * currentDist) + Start;
                return currentEnd;
            }

            return End;
        }

        public bool GrowToWorldPoint(Vector3 worldPoint)
        {
            var newRoot = Root.AddChildRoot(transform.position, worldPoint);
            ChildRoot = newRoot;
            HasRoomForGrowth = ChildRoot == null;
            return !HasRoomForGrowth;
        }

        public void Set(Vector3 start, Vector3 end)
        {
            _collider = GetComponent<CapsuleCollider>();
            HasRoomForGrowth = false;
            Start = start;
            End = end;
            IsGrowing = true;
            _growthPercentage = 0f;
            RefreshSize();
        }

        public void OnEnable()
        {
            _collider = GetComponent<CapsuleCollider>();
        }

        public void RefreshSize()
        {
            if (IsGrowing)
            {
                var currentEnd = CurrentEnd;
                SetTransform(Start, currentEnd);
                SetCollider(Start, currentEnd);
            }
            else
            {
                SetTransform(Start, End);
                SetCollider(Start, End);
            }
        }

        public void Update()
        {
            if (HasRoomForGrowth == false && IsGrowing == false && ChildRoot == null)
            {
                HasRoomForGrowth = true;
            }

            if (IsGrowing)
            {
                if (_growthPercentage < 1f)
                {
                    _growthPercentage += GrowSpeed * Time.deltaTime;
                }
                else
                {
                    IsGrowing = false;
                    _growthPercentage = 1f;
                }
                RefreshSize();
            }
            
        }

        private void SetTransform(Vector3 start, Vector3 end)
        {
            var direction = (end - start).normalized;
            var dist = Vector3.Distance(start, end);
            var middlePoint = ((dist / 2) * direction) + start;

            transform.position = Vector3.zero;
            transform.localPosition = middlePoint;
            transform.forward = direction;
        }

        private void SetCollider(Vector3 start, Vector3 end)
        {
            var dist = Vector3.Distance(start, end);
            _collider.height = dist;
            // _collider.isTrigger = true;
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