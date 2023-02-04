using Flower;
using UnityEngine;

public class Obstacle : MonoBehaviour, IRootCollision
{
    public bool HandleRootPartCollision(RootPart rootPart, Collider collidingPart)
    {
        Debug.Log("You've been Rock Blocked!");
        return true;
    }
}