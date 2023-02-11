using System;
using Dreamteck.Splines;
using UnityEngine;

namespace _GAME.__Scripts.Truck
{
    public class TruckAngle : MonoBehaviour
    {
        [SerializeField] private TruckController _truckController;

        public float maxAngle = 60;
        public float distanceFollow = 0.5f;
        
        public bool isCrash;
        
        private Transform _rotationFollowerObject;

        private SplineFollower _follower;

        [SerializeField] private TruckCrash _truckCrash;

        SplineSample nextPos;

        public Vector3 crashDirection;
        private Quaternion _crashRotation;

        private void OnEnable()
        {
            isCrash = false;
            
            _follower = _truckController.splineFollower;
            
            if (_rotationFollowerObject == null)
            {
                _rotationFollowerObject = new GameObject(gameObject.name + " Follower").transform;
                _rotationFollowerObject.parent = TruckManager.Instance.truckParent;
            }
        }
        

        private void FixedUpdate()
        {
            if (_truckCrash.canCrash)
            {
                CheckAngle();
            }
        }

        public void CheckAngle()
        {
            var percent = _follower.GetPercent();

            nextPos = _truckController.isBackward ? _follower.Evaluate(percent - distanceFollow) : _follower.Evaluate(percent + distanceFollow);
            
            _rotationFollowerObject.position = nextPos.position;
            _rotationFollowerObject.rotation = nextPos.rotation;
            
            var angle = Quaternion.Angle(_follower.result.rotation,
                _rotationFollowerObject
                    .rotation);
            
            if (isCrash)
            {
                var crashAngle = Quaternion.Angle(_crashRotation, _follower.result.rotation);
                if (crashAngle > maxAngle - 20)
                {
                    _truckCrash.DoCrashWithAngle(crashDirection,4);
                }
            }

            if (!isCrash && angle > maxAngle)
            {
                SetCrash();
            }
        }
        
        public void SetCrash()
        {
            isCrash = true;
            crashDirection = _follower.result.forward;
            _crashRotation = _follower.result.rotation;
            
            Debug.Log("rotation first : " + _crashRotation.eulerAngles);

        }
    }
}