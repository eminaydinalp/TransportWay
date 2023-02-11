using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;

namespace _GAME.__Scripts.Spline
{
    public class SplinePointController : MonoBehaviour
    {
        public SplineComputer currentSpline;

        public SplinePoint[] baseSplinePoints;
        public List<SplinePoint> defaultBaseSplinePoints;

        public List<SplinePoint> _splinePointList;

        public SplineEndPoint splineEndPoint;
        
        public bool isActive;
        public bool isEndPoint;

        public MeshRenderer splineMesh;

        public GameObject splinePoint;

        public float yPos;
        private void Start()
        {
            _splinePointList = currentSpline.GetPoints().ToList();
            baseSplinePoints = _splinePointList.ToArray();
            defaultBaseSplinePoints = currentSpline.GetPoints().ToList();

        }

        public void OpenSplineMesh()
        {
            splineMesh.enabled = false;
            
            DOVirtual.DelayedCall(1.5f, () => splineMesh.enabled = true);
        }

        public void ResetSpline()
        {
            _splinePointList.Clear();

            for (int i = 0; i < defaultBaseSplinePoints.Count; i++)
            {
                _splinePointList.Add(defaultBaseSplinePoints[i]);
            }

            baseSplinePoints = _splinePointList.ToArray();
            
            currentSpline.SetPoints(baseSplinePoints.ToArray());

            isActive = false;
            isEndPoint = false;
            
            splinePoint.SetActive(true);
        }

        public void AddNewPoint(Vector3 splinePoint)
        {
            var pos = splinePoint;
            pos = pos.WithY(yPos);
            _splinePointList = baseSplinePoints.ToList();

            SplinePoint newPoint = new SplinePoint();


            newPoint.normal = Vector3.up;
            newPoint.size = 1;
            newPoint.SetPosition(pos);
            

            _splinePointList.Add(newPoint);

            baseSplinePoints = _splinePointList.ToArray();
            
            currentSpline.SetPoints(baseSplinePoints.ToArray());
        }
    }
}