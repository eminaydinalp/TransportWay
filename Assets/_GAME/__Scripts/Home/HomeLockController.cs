using _GAME.__Scripts.Cam;
using _GAME.__Scripts.Click;
using _GAME.__Scripts.TargetHome;
using _GAME.__Scripts.Truck;
using _GAME.__Scripts.Ui;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using Rentire.Utils;
using TMPro;
using UnityEngine;

namespace _GAME.__Scripts.Home
{
    public class HomeLockController : RMonoBehaviour,IClickable
    {
        public HomeLockSo homeLockSo;
        
        public HomeController lockHomeController;
        public TargetHomeController targetHomeController;

        public GameObject openParent;
        public GameObject lockParent;
        
        public TMP_Text requiredMoneyText;
        public TMP_Text requiredMoneyTextLock;

        public bool isOpen;
        public bool isBuy;

        public string levelHomePref;
        public string levelHomePref2;
        
        public float cameraSize;

        private void OnEnable()
        {
            eventManager.event_CollectionUpdated += HandleChangeMoney;
        }
        
        private void OnDisable()
        {
            if (eventManager)
            {
                eventManager.event_CollectionUpdated -= HandleChangeMoney;
            }
        }

        private void Start()
        {
            isBuy = LocalPrefs.GetBool(levelHomePref);
            isOpen = LocalPrefs.GetBool(levelHomePref2);
            
            DefaultOpen();
            
            HandleChangeMoney();
        }

        public void OpenHomeLock()
        {
            LocalPrefs.SetBool(levelHomePref2, true);
        }


        private void OpenHome()
        {
            lockHomeController.transform.localScale = Vector3.zero;
            targetHomeController.transform.localScale = Vector3.zero;
            
            lockHomeController.gameObject.SetActive(true);
            targetHomeController.gameObject.SetActive(true);

            lockHomeController.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InOutCirc);
            DOVirtual.DelayedCall(1, () => targetHomeController.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InOutCirc));
            GameThemeManager.Instance.SetTheme();
            lockHomeController.splinePointController.OpenSplineMesh();
            
            openParent.SetActive(false);
            lockParent.SetActive(false);
            
            MergeManager.Instance.homeControllers.Add(lockHomeController);
            if (TutorialManager.Instance.handGo.activeInHierarchy)
            {
                TutorialManager.Instance.handGo.SetActive(false);
            }
            TutorialManager.Instance.InvokeNextTutorialStep();
        }
        
        
        public void RequiredMoneyControl()
        {
            if (UserPrefs.GetTotalMoney() >= homeLockSo.requiredMoney)
            {
                LocalPrefs.SetBool(levelHomePref, true);
                isBuy = true;
                OpenHome();
                
                UserPrefs.SetTotalMoney(UserPrefs.GetTotalMoney() - homeLockSo.requiredMoney);
                MoneyManager.Instance.SetMoneyText();
                
                FeedbackManager.Instance.Vibrate(HapticTypes.LightImpact);
            }
        }

        private void HandleChangeMoney()
        {
            if(isBuy) return;
            if (UserPrefs.GetTotalMoney() >= homeLockSo.requiredMoney)
            {
                lockParent.SetActive(false);
                openParent.SetActive(true);
                
                requiredMoneyText.text = homeLockSo.requiredMoney.ToString("0");
            }
            else
            {
                lockParent.SetActive(true);
                openParent.SetActive(false);
                
                requiredMoneyTextLock.text = homeLockSo.requiredMoney.ToString("0");
            }
        }

        private void DefaultOpen()
        {
            if (isBuy)
            {
                lockHomeController.gameObject.SetActive(true);
                targetHomeController.gameObject.SetActive(true);
            
                openParent.SetActive(false);
                lockParent.SetActive(false);
            }
        }


        public void ClickProcess()
        {
            if(isBuy) return;
            Debug.Log("Click Home Loca");
            RequiredMoneyControl();
        }
        
        public void SetCameraSize()
        {
            CameraZoomOut.Instance.cameraSize = cameraSize;
            LocalPrefs.SetFloat(CameraZoomOut.Instance.cameraSizePref, cameraSize);
        }
    }
}
