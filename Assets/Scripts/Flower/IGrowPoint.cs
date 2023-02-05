using UnityEngine;

namespace Flower
{
    public interface IGrowPoint
    {
        public Vector3 GrowPosition { get; }
        public bool GrowToWorldPoint(Vector3 worldPoint);
    }
}