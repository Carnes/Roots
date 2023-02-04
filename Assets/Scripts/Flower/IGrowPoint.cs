using UnityEngine;

namespace Flower
{
    public interface IGrowPoint
    {
        public Vector3 GrowPosition { get; }
        public void GrowToWorldPoint(Vector3 worldPoint);
    }
}