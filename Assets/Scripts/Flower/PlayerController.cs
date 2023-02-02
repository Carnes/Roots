using System;
using Roots;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Flower
{
    public class PlayerController : MonoBehaviour
    {
        public Root MainRoot;

        private const int LEFT_MOUSE_BUTTON = 0;
        private const int RIGHT_MOUSE_BUTTON = 1;
        
        public void Start()
        {
            if (MainRoot == null)
                throw new Exception("MainRoot must be connected via Inspector");
        }

        public void Update()
        {
            
            HandleInput();
        }

        private RaycastHit _mouseClickHit;

        public void HandleInput()
        {
            if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON) && !EventSystem.current.IsPointerOverGameObject())
            {
                HandleMouseClick();
            }            
        }

        private void HandleMouseClick()
        {
            var mouseRay = GameSettings.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mouseRay, out _mouseClickHit, 1000f, GameSettings.Instance.SelectableColliderLayer, QueryTriggerInteraction.Collide))
            {
                // Debug.Log($"Grow to point: {_mouseClickHit.point.ToString()}");
                MainRoot.AddRootWorldPoint(_mouseClickHit.point);
            }
        }
    }
}