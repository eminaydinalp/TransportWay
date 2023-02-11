using _GAME.__Scripts.Truck;
using Dreamteck.Splines;
using UnityEngine;

namespace _GAME.__Scripts.Spline
{
    public class TruckMovement
    {
        SplineFollower _splineFollower;
        
        public TruckMovement(SplineFollower splineFollower)
        {
            _splineFollower = splineFollower;
        }

        public void SplineRestart()
        {
            _splineFollower.Restart();
        }

        public void SetSplineComputer(SplineComputer splineComputer)
        {
            _splineFollower.spline = splineComputer;
        }
        

        public void Move()
        {
            _splineFollower.followDuration = TruckManager.Instance.currentSpeed;
        }
    }
}