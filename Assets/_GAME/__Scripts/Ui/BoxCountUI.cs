using System.Collections;
using _GAME.__Scripts.Home;
using DG.Tweening;
using Rentire.Core;
using Rentire.Utils;
using TMPro;
using UnityEngine;

namespace _GAME.__Scripts.Ui
{
    public class BoxCountUI : Singleton<BoxCountUI>
    {
        public TargetPackageSo targetPackageSo;
        public int targetCount;
        
        public string targetBoxPref;

        public int currentCount;

        public TMP_Text boxText;
        public TMP_Text completedText;

        private Tweener _tweener;

        [SerializeField] GameObject animatedCoinPrefab;
        [SerializeField] RectTransform target;

        [Space] [Header("Animation settings")] [SerializeField] [Range(0.5f, 0.9f)]
        float minAnimDuration;

        [SerializeField] [Range(0.9f, 2f)] float maxAnimDuration;

        [SerializeField] Ease easeType;
        [SerializeField] float spread;

        Vector3 targetPosition;

        public string boxCountUIPref;


        private void Start()
        {
            targetCount = LocalPrefs.GetInt(targetBoxPref, targetPackageSo.targetCount);
            currentCount = LocalPrefs.GetInt(boxCountUIPref, currentCount);
            ShowBoxText();
        }

        private void ShowBoxText()
        {
            boxText.text = currentCount + " / " + targetCount;
        }

        public void IncreaseBoxCount(int increasedValue)
        {
            int newCurrentCount = currentCount;
            newCurrentCount += increasedValue;

            if (newCurrentCount > targetCount)
            {
                newCurrentCount = targetCount;
            }

            _tweener.Kill();

            _tweener = DOVirtual.Float(currentCount, newCurrentCount, 1f, value =>
            {
                currentCount = (int)value;
                ShowBoxText();
            });

            _tweener.OnComplete((() => { TargetBoxControl(); }));
        }

        private void TargetBoxControl()
        {
            LocalPrefs.SetInt(boxCountUIPref, currentCount);
            
            if (currentCount >= targetCount)
            {
                targetCount += targetCount * targetPackageSo.multipleIncrease + targetPackageSo.plusValue;
                LocalPrefs.SetInt(targetBoxPref, targetCount);
                currentCount = 0;
                LocalPrefs.SetInt(boxCountUIPref, currentCount);

                ShowBoxText();
                ShowCompletedText();
            }
        }

        private void ShowCompletedText()
        {
            completedText.gameObject.SetActive(true);

            completedText.transform.DOScale(Vector3.one * 3, 1f);
            completedText.DOFade(1, 1f).OnComplete((() =>
            {
                completedText.DOFade(0, 2f);
                completedText.transform.DOScale(Vector3.zero, 2f);

                HomeLockManager.Instance.OpenNewHomeLock();
            }));
        }

        public IEnumerator Animate(Vector3 collectedCoinPosition, int amount)
        {
            for (int i = 0; i < 5; i++)
            {
                //GameObject coin = Instantiate(animatedCoinPrefab , target.parent);

                GameObject coin = PoolManager.Instance.Spawn_Object(PoolsEnum.PackageUI, target.parent, false, 1f);
                
                var point = RectExtensions.WorldPointToCanvasLocalRectTransformPoint(collectedCoinPosition, Camera.main,
                    CamManager.Instance.MasterCanvas,
                    target);
                
                
                coin.transform.localPosition = point;
                
                
                // coin.transform.position = collectedCoinPosition + new Vector3(1, -2f, 0);
                // coin.transform.SetParent(transform);
                //coin.transform.localRotation = Quaternion.Euler(Vector3.zero);
                //coin.transform.localScale = Vector3.one * 0.4f;

                
                coin.transform.DOMove(target.position, minAnimDuration)
                    .SetEase(easeType)
                    .OnComplete(() =>
                    {
                        target.DOPunchScale(0.2f * Vector3.one, 0.1f, 0, 0.2f);
                    });

                yield return new WaitForSeconds(0.1f);

            }
            
        }
        
        public Vector3 GetUıPosition(Vector3 target)
        {
            Vector3 uiPos = targetPosition;

            uiPos.z = (target - Camera.main.transform.position).z;

            Vector3 result = Camera.main.ScreenToWorldPoint(uiPos);

            return result;
        }
    }
}