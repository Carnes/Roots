using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using Unity.VisualScripting;
using UnityEngine;

namespace Roots
{
    [RequireComponent(typeof(LineRenderer))]
    public class Root : MonoBehaviour
    {
        public float RootWidthMin = 0.5f;
        public float RootWidthMax = 1.5f;
        public float RootWidthPerSegment = 0.25f;
        public List<Vector3> Points;
        public Vector3 GrowDirectionMin;
        public Vector3 GrowDirectionMax;
        public Vector3 BoundaryMin;
        public Vector3 BoundaryMax;
        
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

        public void SetRootWidth()
        {
            var rootWidth = RootWidthPerSegment * Points.Count;
            if (rootWidth > RootWidthMax)
                rootWidth = RootWidthMax;
            if (rootWidth < RootWidthMin)
                rootWidth = RootWidthMin;
            _lineRenderer.startWidth = rootWidth;
        }

        public void AddRootPoint(Vector3 point)
        {
            Points.Add(point);
            _lineRenderer.positionCount = Points.Count;
            _lineRenderer.SetPosition(Points.Count-1, point);
            SetRootPoints();
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            GizmoHelper.DrawRectangle(BoundaryMin, BoundaryMax, transform.position);
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