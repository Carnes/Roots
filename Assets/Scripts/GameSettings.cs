using System.Collections.Generic;
using Flower;
using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Roots
{
    public class GameSettings : Helpers.Singleton<GameSettings>
    {
        public Vector3 BoundaryMin;
        public Vector3 BoundaryMax;
        public Vector3 PositionOfGameBoard = Vector3.zero;
        public LayerMask SelectableColliderLayer;
        public Camera MainCamera;
        [Header("Prefabs")]
        public GameObject RootPartPrefab;
        public GameObject RootDeathPrefab;
        public GameObject RootPrefab;        

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

        public void RestartGame()
        {
            SceneManager.LoadScene("Prototype");
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }          

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            GizmoHelper.DrawRectangle(BoundaryMin, BoundaryMax, PositionOfGameBoard);
        }        
    }
}