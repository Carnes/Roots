using System;
using System.Linq;
using Roots;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Random = System.Random;
using Random2 = UnityEngine.Random;

namespace Flower
{
    public class PlayerController : MonoBehaviour
    {
        public Root MainRoot;
        public GameObject NearestGrowPoint;

        private RootNutrientReserve _nutrientReserve;
        private const int LEFT_MOUSE_BUTTON = 0;
        private const int RIGHT_MOUSE_BUTTON = 1;
        
        public void Start()
        {
            if (MainRoot == null)
                throw new Exception("MainRoot must be connected via Inspector");

            _nutrientReserve = RootNutrientReserve.Instance;
        }

        public void Update()
        {
            var mousePosition = GetMousePosition();
            if (mousePosition != null)
            {
                var nearestPoint = GetNearestGrowPoint(mousePosition.Value);
                if (nearestPoint != null)
                {
                    NearestGrowPoint.SetActive(true);
                    NearestGrowPoint.transform.position = nearestPoint.GrowPosition;
                }
                else
                {
                    NearestGrowPoint.SetActive(false);
                }
            }
            else
            {
                NearestGrowPoint.SetActive(false);
            }
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

        IGrowPoint GetNearestGrowPoint(Vector3 point) // FIXME - this could be cached and refresh after roots added/destroyed
        {
            var growPoints = MainRoot.GetGrowablePoints();
            if (growPoints.Any() == false)
                return null;
            var nearestPoint = growPoints.First();
            var minDist = float.MaxValue;
            foreach (var growPoint in growPoints)
            {
                var dist = Vector3.Distance(point, growPoint.GrowPosition);
                if (dist < minDist)
                {
                    nearestPoint = growPoint;
                    minDist = dist;
                }
            }

            return nearestPoint;
        }

        private Vector3? GetMousePosition()
        {
            var mouseRay = GameSettings.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out _mouseClickHit, 1000f, GameSettings.Instance.SelectableColliderLayer, QueryTriggerInteraction.Collide))
                return _mouseClickHit.point;
            return null;
        }

        private void HandleMouseClick()
        {
            var mousePoint = GetMousePosition();
            if (mousePoint != null)
            {
                if (_nutrientReserve.NutrientsInReserve >= 0)
                {
                    var growPoint = GetNearestGrowPoint(_mouseClickHit.point);
                    if(growPoint != null)
                        growPoint.GrowToWorldPoint(_mouseClickHit.point);
                }
                else
                {
                    Debug.Log("Not enough nutrients to move the root!");
                }
            }
        }
    }
}