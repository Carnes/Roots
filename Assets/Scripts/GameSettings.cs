using Helpers;
using UnityEngine;

namespace Roots
{
    public class GameSettings : Helpers.Singleton<GameSettings>
    {
        public Vector3 BoundaryMin;
        public Vector3 BoundaryMax;
        public Vector3 PositionOfGameBoard = Vector3.zero;
        public LayerMask SelectableColliderLayer;
        public Camera MainCamera;

        public override void SingletonAwake()
        {            
            Application.targetFrameRate = 300;
            QualitySettings.vSyncCount = 0;  
        }

        public override void SingletonStart()
        {
            if (PositionOfGameBoard == Vector3.zero)
                PositionOfGameBoard = transform.position;
            if(MainCamera == null)
                MainCamera = Camera.main;
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            GizmoHelper.DrawRectangle(BoundaryMin, BoundaryMax, PositionOfGameBoard);
        }        
    }
}