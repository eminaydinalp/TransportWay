using _GAME.__Scripts.Click;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace _GAME.__Scripts.Spline
{
    public class SplineActive : MonoBehaviour,IClickable
    {
        public SplinePointController splinePointController;
        
        public void ClickProcess()
        {
            SplineManager.Instance.activeSplinePointController = splinePointController;
            SplineManager.Instance.activeSplinePointController.isActive = true;
            
            splinePointController.splinePoint.SetActive(false);
            
            FeedbackManager.Instance.Vibrate(HapticTypes.LightImpact);
        }
    }
}
