using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Helpers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Flower
{
    [RequireComponent(typeof(LineRenderer))]
    public class Root : MonoBehaviour
    {
        [InspectorLabel("Prefabs")]
        public GameObject RootPartGameObject;
        public GameObject RootDeathGameObject;

        [InspectorLabel("Settings")]
        public float RootWidthMin = 0.5f;
        public float RootWidthMax = 1.5f;
        public float RootWidthPerSegment = 0.25f;
        
        [InspectorLabel("Starting points")]
        public List<Vector3> StartingStaticPoints = new List<Vector3>();

        private List<RootPart> RootParts = new List<RootPart>();
        private Root ParentRoot;
        private List<Root> ChildrenRoots = new List<Root>();
        private LineRenderer _lineRenderer;

        private List<Vector3> AllPoints => StartingStaticPoints.Union(RootParts.Select(rp=>rp.End).ToList()).ToList();

        public void OnEnable()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.useWorldSpace = false;
        }

        public void Start()
        {
            SetRootPoints();
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
   
        public void AddRootWorldPoint(Vector3 worldPoint)
        {
            var localPoint = transform.InverseTransformPoint(worldPoint);
                AddRootPoint(localPoint);
        }    

        // public int GetCountOfRootPartsFromFlower(int currentCount = 0)
        // {
        //     currentCount++;
        //     if (ParentRoot == null)
        //         return currentCount;
        //     return ParentRoot.GetCountOfRootPartsFromFlower(currentCount);
        // }

        private int GetCountOfRootPartsFromChildren()
        {
            var currentCount = AllPoints.Count;
            foreach (var child in ChildrenRoots)
            {
                currentCount += child.GetCountOfRootPartsFromChildren();
            }

            return currentCount;
        }
 

        private void SetRootWidth()
        {
            var childParts = GetCountOfRootPartsFromChildren();
            var rootWidth = RootWidthPerSegment * (childParts);
            if (rootWidth > RootWidthMax)
                rootWidth = RootWidthMax;
            if (rootWidth < RootWidthMin)
                rootWidth = RootWidthMin;
            _lineRenderer.startWidth = rootWidth;
        }


        private void AddRootPoint(Vector3 point)
        {
            var lastPoint = AllPoints.LastOrDefault();
            var rootPart = CreateRootPart(lastPoint, point);
            RootParts.Add(rootPart);

            var allPoints = AllPoints;
            _lineRenderer.positionCount = allPoints.Count;
            _lineRenderer.SetPosition(allPoints.Count-1, point);
            
            SetRootPoints();
        }
        
        private void SetRootPoints()
        {
            var allPoints = AllPoints;
            var offSet = transform.position;
            _lineRenderer.positionCount = allPoints.Count;
            _lineRenderer.SetPositions(allPoints.ToArray());
            SetRootWidth();
        }

        private RootPart CreateRootPart(Vector3 start, Vector3 end)
        {
            var rootPartGameObject = Instantiate(RootPartGameObject, transform);
            var rootPart = rootPartGameObject.GetComponent<RootPart>();
            rootPart.Set(start, end);
            rootPartGameObject.SetActive(true);
            return rootPart;
        }
        
        private void DestroyRootPart(RootPart rootPart)
        {
            RootParts.Remove(rootPart);
            var rootDeath = Instantiate(RootDeathGameObject, rootPart.gameObject.transform.position, Quaternion.identity);
            rootDeath.SetActive(true);
            Destroy(rootPart.gameObject, 0.01f); // FIXME - magic number
        }
    }
}