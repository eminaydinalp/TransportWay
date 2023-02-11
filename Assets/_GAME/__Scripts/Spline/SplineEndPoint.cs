using _GAME.__Scripts.Drag;
using _GAME.__Scripts.Home;
using _GAME.__Scripts.TargetHome;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace _GAME.__Scripts.Spline
{
    public class SplineEndPoint : MonoBehaviour
    {
        [SerializeField] private SplinePointController _splinePointController;
        [SerializeField] private HomeController _homeController;
        [SerializeField] private RemoveSpline removeSpline;
        
        TargetHomeController targetHomeControllerNew;
        public void StartInvoke()
        {
            InvokeRepeating(nameof(EndPointControl), 0, 0.2f);
        }

        public void EndPointControl()
        {
            Collider[] colliders = Physics.OverlapSphere(DragManager.Instance.dragPositions[^1], 0.5f, LayerMask.GetMask("TargetSphere"));
            
            if(colliders.Length <= 0) return;
            
            if (colliders[0].transform.parent.TryGetComponent(out TargetHomeController targetHomeController))
            {
                targetHomeControllerNew = targetHomeController;
            }
            
            if (targetHomeControllerNew.truckColor == _homeController.truckColor)
            {
                EventManager.Instance.InvokeOnEndHome();
                var roadPos = targetHomeControllerNew.roadEndPos.transform.position.AddZ(1);
                DragManager.Instance.dragPositions.Add(roadPos);
                _splinePointController.AddNewPoint(roadPos);
                _splinePointController.isEndPoint = true;
                _splinePointController.isActive = false;
                removeSpline.OpenRemoveSplineSprite();
                _homeController.InvokeMoveTruck();
                CloseInvoke();
                FeedbackManager.Instance.Vibrate(HapticTypes.LightImpact);
            }
        }
        
        public void CloseInvoke()
        {
            CancelInvoke(nameof(EndPointControl));
        }
    }
}