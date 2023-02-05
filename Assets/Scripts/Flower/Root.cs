using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Helpers;
using Roots;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Flower
{
    [RequireComponent(typeof(LineRenderer))]
    public class Root : MonoBehaviour, IGrowPoint
    {
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

        private List<Vector3> AllPoints => StartingStaticPoints.Union(RootParts.Select(rp=>rp.CurrentEnd).ToList()).ToList();

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
            var lastPart = RootParts.LastOrDefault();
            
            if(lastPart == null || lastPart.IsGrowing == false)
                points.Add(this);
            
            foreach (var rootPart in RootParts)
            {
                if(rootPart.HasRoomForGrowth && rootPart.IsGrowing == false)
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
            var rootGameObject = Instantiate(GameSettings.Instance.RootPrefab, transform);
            var root = rootGameObject.GetComponent<Root>();
            var localStart = transform.InverseTransformPoint(start);
            rootGameObject.transform.localPosition = localStart;
            rootGameObject.SetActive(true);
            var isSuccess = root.GrowToWorldPoint(end);
            if (!isSuccess)
            {
                Destroy(rootGameObject, 0.01f);
                return null;
            }
            else
            {
                ChildrenRoots.Add(root);
            }

            return root;
        }

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

        private void Update()
        {
            RefreshLineRenderer();
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
            var rootPartGameObject = Instantiate(GameSettings.Instance.RootPartPrefab, transform);
            var rootPart = rootPartGameObject.GetComponent<RootPart>();
            rootPart.Set(start, end);
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
            var rootDeath = Instantiate(GameSettings.Instance.RootDeathPrefab, rootPart.gameObject.transform.position, Quaternion.identity);
            rootDeath.SetActive(true);
            Destroy(rootPart.gameObject, 0.01f); // FIXME - magic number
        }
    }
}