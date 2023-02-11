using System.Collections.Generic;
using _GAME.__Scripts.Home;
using _GAME.__Scripts.Spline;
using MoreMountains.NiceVibrations;
using Rentire.Core;
using UnityEngine;

namespace _GAME.__Scripts.Drag
{
    public class DragManager : Singleton<DragManager>
    {
        public List<Vector3> dragPositions;
        public List<Vector3> defaultDragPositions;
        
        [SerializeField] private Camera _camera;

        private void OnEnable()
        {
            EventManager.OnSplineReset += ResetDragPos;
        }
        private void OnDisable()
        {
            EventManager.OnSplineReset -= ResetDragPos;
        }

        public void AddFirstValue()
        {
            if(!SplineManager.Instance.activeSplinePointController.isActive) return;
            
            Ray ray = _camera.ScreenPointToRay(InputManager.Instance.firstTouchPosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 200f, LayerMask.GetMask("Ground")))
            {

                // if (Mathf.Abs(dragPositions[^1].magnitude - hitInfo.point.magnitude) > 0.5f)
                // {
                //     dragPositions.Add(hitInfo.point);
                //     SplineManager.Instance.activeSplinePointController.AddNewPoint(hitInfo.point);
                // }
                
                if (Vector3.Distance(dragPositions[^1] ,hitInfo.point) > 0.5f)
                {
                    dragPositions.Add(hitInfo.point);
                    SplineManager.Instance.activeSplinePointController.AddNewPoint(hitInfo.point);
                    FeedbackManager.Instance.Vibrate(HapticTypes.LightImpact);
                }
            }
        }

        public void ResetDragPos()
        {
            dragPositions.Clear();
            
            for (int i = 0; i < defaultDragPositions.Count; i++)
            {
                dragPositions.Add(defaultDragPositions[i]);
            }
        }
        
        
        
    }
}