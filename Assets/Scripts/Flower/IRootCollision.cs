using UnityEngine;

namespace Flower
{
    public interface IRootCollision
    {
        public bool HandleRootPartCollision(RootPart rootPart, Collider collidingPart);
    }
}