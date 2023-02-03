using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Helpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Flower
{
    [RequireComponent(typeof(LineRenderer))]
    public class Root : MonoBehaviour
    {
        public float RootWidthMin = 0.5f;
        public float RootWidthMax = 1.5f;
        public float RootWidthPerSegment = 0.25f;
        public List<Vector3> StartingStaticPoints;
        public List<Vector3> AllPoints => StartingStaticPoints.Union(RootParts.Select(rp=>rp.End).ToList()).ToList();
        
        public List<RootPart> RootParts;
        public GameObject RootPartGameObject;
        public GameObject RootDeathGameObject;

        public Root ParentRoot;
        public List<Root> ChildrenRoots;
        
        private LineRenderer _lineRenderer;

        public void OnEnable()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.useWorldSpace = false;
        }

        public void Start()
        {
            SetRootPoints();
        }

        public void SetRootPoints()
        {
            var allPoints = AllPoints;
            var offSet = transform.position;
            _lineRenderer.positionCount = allPoints.Count;
            _lineRenderer.SetPositions(allPoints.ToArray());
            SetRootWidth();
        }

        public void RootPartHit(RootPart rootPartThatWasHit)
        {
            var isDestroying = false;
            foreach (var rootPart in RootParts.ToList())
            {
                if (isDestroying)
                {
                    DestroyRootPart(rootPart);
                }
                else if (rootPart == rootPartThatWasHit)
                {
                    isDestroying = true;
                    DestroyRootPart(rootPart);
                }
            }
            SetRootPoints();
        }

        private void DestroyRootPart(RootPart rootPart)
        {
            RootParts.Remove(rootPart);
            var rootDeath = Instantiate(RootDeathGameObject, rootPart.gameObject.transform.position, Quaternion.identity);
            rootDeath.SetActive(true);
            Destroy(rootPart.gameObject, 0.01f); // FIXME - magic number
        }

        // public int GetCountOfRootPartsFromFlower(int currentCount = 0)
        // {
        //     currentCount++;
        //     if (ParentRoot == null)
        //         return currentCount;
        //     return ParentRoot.GetCountOfRootPartsFromFlower(currentCount);
        // }

        public int GetCountOfRootPartsFromChildren()
        {
            var currentCount = AllPoints.Count;
            foreach (var child in ChildrenRoots)
            {
                currentCount += child.GetCountOfRootPartsFromChildren();
            }

            return currentCount;
        }
        

        public void SetRootWidth()
        {
            var childParts = GetCountOfRootPartsFromChildren();
            var rootWidth = RootWidthPerSegment * (childParts);
            if (rootWidth > RootWidthMax)
                rootWidth = RootWidthMax;
            if (rootWidth < RootWidthMin)
                rootWidth = RootWidthMin;
            _lineRenderer.startWidth = rootWidth;
        }

        public void AddRootWorldPoint(Vector3 worldPoint)
        {
            var localPoint = transform.InverseTransformPoint(worldPoint);
                AddRootPoint(localPoint);
        }

        public void AddRootPoint(Vector3 point)
        {
            var lastPoint = AllPoints.LastOrDefault();

            var rootPartGameObject = Instantiate(RootPartGameObject, transform);
            var rootPart = rootPartGameObject.GetComponent<RootPart>();
            rootPart.Set(lastPoint, point);
            rootPartGameObject.SetActive(true);
            
            RootParts.Add(rootPart);

            var allPoints = AllPoints;
            _lineRenderer.positionCount = allPoints.Count;
            _lineRenderer.SetPosition(allPoints.Count-1, point);
            
            SetRootPoints();
        }

        [ContextMenu("Grow Down")]
        public void GrowDown()
        {
            var down = Vector3.down * 0.50f; // FIXME - magic number
            var endPoint = AllPoints.LastOrDefault();
            AddRootPoint(endPoint + down);
        }
    }
}