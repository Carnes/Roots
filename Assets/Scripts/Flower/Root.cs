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
    public class Root : MonoBehaviour, IGrowPoint
    {
        [FormerlySerializedAs("RootPartGameObject")] [Header("Prefabs")]
        public GameObject RootPartPrefab;
        public GameObject RootDeathPrefab;
        public GameObject RootPrefab;

        [Header("Settings")]
        public float RootWidthMin = 0.5f;
        public float RootWidthMax = 1.5f;
        public float RootWidthPerSegment = 0.25f;
        
        [Header("Starting points")]
        public List<Vector3> StartingStaticPoints = new List<Vector3>();

        private List<RootPart> RootParts = new List<RootPart>();
        private Root ParentRoot;
        private List<Root> ChildrenRoots = new List<Root>();
        private LineRenderer _lineRenderer;

        public Vector3 GrowPosition => AllPoints.Last() + transform.position  + new Vector3(0,0,-0.25f); // very end of root

        private List<Vector3> AllPoints => StartingStaticPoints.Union(RootParts.Select(rp=>rp.End).ToList()).ToList();

        public void OnEnable()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.useWorldSpace = false;
        }

        public void Start()
        {
            RefreshLineRenderer();
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
            RefreshLineRenderer();
        }

        public void RootHit()
        {
            var topRootPart = RootParts.FirstOrDefault();
            if (topRootPart != null)
            {
                RootPartHit(topRootPart);
            }
            Destroy(this.gameObject, 0.1f);
        }

        public List<IGrowPoint> GetGrowablePoints()
        {
            var points = new List<IGrowPoint>();
            points.Add(this);
            foreach (var rootPart in RootParts)
            {
                if(rootPart.HasRoomForGrowth)
                    points.Add(rootPart);
            }

            foreach (var child in ChildrenRoots)
            {
                points.AddRange(child.GetGrowablePoints());
            }

            return points;
        }

        public bool GrowToWorldPoint(Vector3 worldPoint)
        {
            var localPoint = transform.InverseTransformPoint(worldPoint);
            var localPointNormalized = new Vector3(localPoint.x, localPoint.y, 0f);
            return AddRootPoint(localPointNormalized);
        }

        public Root AddChildRoot(Vector3 start, Vector3 end)
        {
            var rootGameObject = Instantiate(RootPrefab, transform);
            var root = rootGameObject.GetComponent<Root>();
            ChildrenRoots.Add(root);
            // root.StartingStaticPoints = new List<Vector3> { new Vector3(0,0,0) };
            var localStart = transform.InverseTransformPoint(start);
            rootGameObject.transform.localPosition = localStart;
            var isSuccess = root.GrowToWorldPoint(end);
            if (!isSuccess)
            {
                Destroy(rootGameObject);
                return null;
            }

            return root;
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

        private bool AddRootPoint(Vector3 point)
        {
            var lastPoint = AllPoints.Last();
            var dist = Vector3.Distance(point, lastPoint);
            if (dist < 0.25f) return false; // don't grow tiny roots 
            RootNutrientReserve.Instance.SubtractNutrientByDistance(dist);
            
            var rootPart = CreateRootPart(lastPoint, point);
            RootParts.Add(rootPart);

            var allPoints = AllPoints;
            _lineRenderer.positionCount = allPoints.Count;
            _lineRenderer.SetPosition(allPoints.Count-1, point);
            
            RefreshLineRenderer();
            return true;
        }
        
        private void RefreshLineRenderer()
        {
            var allPoints = AllPoints;
            _lineRenderer.positionCount = allPoints.Count;
            _lineRenderer.SetPositions(allPoints.ToArray());
            SetRootWidth();
        }

        private RootPart CreateRootPart(Vector3 start, Vector3 end)
        {
            var rootPartGameObject = Instantiate(RootPartPrefab, transform);
            var rootPart = rootPartGameObject.GetComponent<RootPart>();
            rootPart.Set(start, end, true);
            rootPartGameObject.SetActive(true);
            return rootPart;
        }
        
        private void DestroyRootPart(RootPart rootPart)
        {
            if (rootPart.ChildRoot != null)
            {
                ChildrenRoots.Remove(rootPart.ChildRoot);
                rootPart.ChildRoot.RootHit();
            }

            RootParts.Remove(rootPart);
            var rootDeath = Instantiate(RootDeathPrefab, rootPart.gameObject.transform.position, Quaternion.identity);
            rootDeath.SetActive(true);
            Destroy(rootPart.gameObject, 0.01f); // FIXME - magic number
        }
    }
}