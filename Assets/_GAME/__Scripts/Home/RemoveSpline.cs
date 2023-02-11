using _GAME.__Scripts.Click;
using _GAME.__Scripts.Drag;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace _GAME.__Scripts.Home
{
    public class RemoveSpline : MonoBehaviour,IClickable
    {
        [SerializeField] private HomeController homeController;

        [SerializeField] private SpriteRenderer removeSprite;

        private void Start()
        {
            CloseRemoveSplineSprite();
        }
        
        private void OnEnable()
        {
            EventManager.OnSplineReset += CloseRemoveSplineSprite;
        }
        private void OnDisable()
        {
            EventManager.OnSplineReset -= CloseRemoveSplineSprite;
        }

        public void RemoveSelfSpline()
        {
            DragManager.Instance.ResetDragPos();
            
            homeController.CancelMoveTruck();
            homeController.ResetPos();
            homeController.ResetSpline();
            homeController.RemoveTrucks();
            
            CloseRemoveSplineSprite();
        }

        public void OpenRemoveSplineSprite()
        {
            removeSprite.gameObject.SetActive(true);
        }

        private void CloseRemoveSplineSprite()
        {
            removeSprite.gameObject.SetActive(false);
        }

        public void ClickProcess()
        {
            FeedbackManager.Instance.Vibrate(HapticTypes.LightImpact);
            RemoveSelfSpline();
        }

        public void Punch()
        {
            transform.DOPunchScale(Vector3.one * 0.2f, 1f).SetEase(Ease.InBounce);
        }
    }
}