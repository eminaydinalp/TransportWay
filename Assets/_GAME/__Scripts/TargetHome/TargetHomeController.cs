using _GAME.__Scripts.Click;
using _GAME.__Scripts.Home;
using _GAME.__Scripts.Truck;
using _GAME.__Scripts.Ui;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using Rentire.Utils;
using TMPro;
using UnityEngine;

namespace _GAME.__Scripts.TargetHome
{
    public class TargetHomeController : RMonoBehaviour,IClickable
    {
        public TargetAreaCapacitySo targetAreaCapacitySo;
        
        public int currentCount;

        public TMP_Text boxText;
        public SpriteRenderer clickSprite;

        private Tweener _tweener;
        private bool _isFull;

        public TruckColor truckColor;

        public int difference;

        public SpriteRenderer outlineSprite;
        public SpriteRenderer innerSprite;

        public Transform roadEndPos;

        public string packageCountPref;

        private void Start()
        {
            currentCount = LocalPrefs.GetInt(packageCountPref, currentCount);
            
            ShowBoxText();
            if (UserPrefs.GetMaxHomeLevel() > 0)
            {
                boxText.transform.parent.DOScale(boxText.transform.parent.localScale + ((Vector3.one * UserPrefs.GetMaxHomeLevel()) * 0.2f), 0.1f);
            }
        }
        
        private void OnEnable()
        {
            HomeLockManager.OnHomeUnlocked += ScaleUpBoxText;
        }

        private void OnDisable()
        {
            HomeLockManager.OnHomeUnlocked -= ScaleUpBoxText;
        }

        private void ScaleUpBoxText()
        {
            boxText.transform.parent.DOScale(boxText.transform.parent.localScale + (Vector3.one * 0.2f), 0.1f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                TruckController truckController = other.GetComponent<TruckController>();
                truckController.isBackward = true;
                
                if(truckController.currentPackageCount <= 0 || _isFull || truckColor != truckController.truckColor) return;
                
                IncreaseBoxCount(truckController.currentPackageCount);
                MoneyManager.Instance.IncreaseMoney(truckController.currentPackageCount * truckController.truckSo.boxMultiplyMoney);
                truckController.truckMoneyText.OpenMoneyText(truckController.currentPackageCount * truckController.truckSo.boxMultiplyMoney, transform.position);
                
                truckController.stackController.StackReset();
                
                TargetHomeShake();
                // if (UserPrefs.GetTutorial() && TutorialManager.Instance.CurrentTutorialStep == 3)
                // {
                //     TutorialManager.Instance.InvokeNextTutorialStep();
                // }
                
                FeedbackManager.Instance.Vibrate(HapticTypes.LightImpact);
            }
        }

        private void TargetHomeShake()
        {
            transform.DOShakeScale(0.5f, Vector3.one * 0.2f);
        }

        private void IncreaseBoxCount(int increasedValue)
        {
            int newCurrentCount = currentCount;
            newCurrentCount += increasedValue;
            
            
            if (newCurrentCount > targetAreaCapacitySo.targetCount)
            {
                difference = newCurrentCount - targetAreaCapacitySo.targetCount;
                newCurrentCount = targetAreaCapacitySo.targetCount;
            }
            

            currentCount = newCurrentCount;

            LocalPrefs.SetInt(packageCountPref, currentCount);
            
            if (currentCount >= targetAreaCapacitySo.targetCount)
            {
                DoFull();
                ShowBoxText();
            }
            
            ShowBoxText();
        }

        private void DoFull()
        {
            _isFull = true;
            clickSprite.gameObject.SetActive(true);
            boxText.gameObject.SetActive(false);
            clickSprite.transform.DOPunchScale(Vector3.one * 0.05f, 0.3f).SetLoops(-1).SetEase(Ease.InOutCirc);
        }
        
        private void ShowBoxText()
        {
            boxText.text = currentCount + " / " + targetAreaCapacitySo.targetCount;
        }

        private void ClickBox()
        {
            if(!_isFull) return;
            
            clickSprite.gameObject.SetActive(false);
            boxText.gameObject.SetActive(true);
            clickSprite.transform.DOKill();
            BoxCountUI.Instance.IncreaseBoxCount(currentCount);


            StartCoroutine(BoxCountUI.Instance.Animate(transform.position, currentCount));
            currentCount = difference;
            LocalPrefs.SetInt(packageCountPref, currentCount);
            difference = 0;
            ShowBoxText();
            _isFull = false;
        }

        public void ClickProcess()
        {
            ClickBox();
        }

        
    }
}
