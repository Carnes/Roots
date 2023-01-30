using UnityEngine;

namespace Helpers
{
    public static class GizmoHelper
    {
        public static void DrawRectangle(Vector3 boundaryMin, Vector3 boundaryMax, Vector3 offSet)
        {
            var frontBottomLeft = boundaryMin + offSet;
            var frontTopLeft = new Vector3(boundaryMin.x, boundaryMax.y, boundaryMin.z) + offSet;
            var frontTopRight = new Vector3(boundaryMax.x, boundaryMax.y, boundaryMin.z) + offSet;
            var frontBottomRight = new Vector3(boundaryMax.x, boundaryMin.y, boundaryMin.z) + offSet;

            var backTopRight = boundaryMax + offSet;
            var backBottomRight = new Vector3(boundaryMax.x, boundaryMin.y, boundaryMax.z) + offSet;
            var backBottomLeft = new Vector3(boundaryMin.x, boundaryMin.y, boundaryMax.z) + offSet;
            var backTopLeft = new Vector3(boundaryMin.x, boundaryMax.y, boundaryMax.z) + offSet;
            
            // Gizmos.color = Color.red;
            
            //Draw front
            Gizmos.DrawLine(frontBottomLeft, frontTopLeft);
            Gizmos.DrawLine(frontTopLeft, frontTopRight);
            Gizmos.DrawLine(frontTopRight, frontBottomRight);
            Gizmos.DrawLine(frontBottomRight, frontBottomLeft);
            
            //Draw back
            Gizmos.DrawLine(backBottomLeft, backTopLeft);
            Gizmos.DrawLine(backTopLeft, backTopRight);
            Gizmos.DrawLine(backTopRight, backBottomRight);
            Gizmos.DrawLine(backBottomRight, backBottomLeft);
            
            //Connect front to back
            Gizmos.DrawLine(frontTopLeft, backTopLeft);
            Gizmos.DrawLine(frontTopRight, backTopRight);
            Gizmos.DrawLine(frontBottomRight, backBottomRight);
            Gizmos.DrawLine(frontBottomLeft, backBottomLeft);
        }        
    }
}