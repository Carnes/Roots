using System.Collections.Generic;
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
        public List<Vector3> Points;

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
            var offSet = transform.position;
            _lineRenderer.positionCount = Points.Count;
            _lineRenderer.SetPositions(Points.ToArray());
            SetRootWidth();
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
            var currentCount = Points.Count;
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
            Points.Add(point);
            _lineRenderer.positionCount = Points.Count;
            _lineRenderer.SetPosition(Points.Count-1, point);
            SetRootPoints();
        }

        [ContextMenu("Grow Down")]
        public void GrowDown()
        {
            var down = Vector3.down * 0.50f; // FIXME - magic number
            var endPoint = Points.LastOrDefault();
            AddRootPoint(endPoint + down);
        }
    }
}