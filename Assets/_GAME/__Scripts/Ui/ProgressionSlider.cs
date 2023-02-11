using DG.Tweening;
using Rentire.Core;
using Rentire.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace _GAME.__Scripts.Ui
{
    public class ProgressionSlider : Singleton<ProgressionSlider>
    {
        public Slider levelSlider;

        public int firstMoney;

        public int maxValue;

        public Image fillImage;

        public int totalMoney;

        private void Start()
        {
            firstMoney = UserPrefs.GetTotalMoney();
            totalMoney = firstMoney;
            DOVirtual.DelayedCall(0.5f, () => SetSliderMaxValue(FindObjectOfType<Level>().requiredMoney));
        }
    
        private void OnEnable()
        {
            EventManager.Instance.event_CollectionUpdated += HandleChangeMoney;
        }
        
        private void OnDisable()
        {
            EventManager.Instance.event_CollectionUpdated -= HandleChangeMoney;
        }

        private void UpdateSlider(float value)
        {
            fillImage.fillAmount = value;

            if (fillImage.fillAmount >= 1)
            {
                GameManager.Instance.SetGameSuccess();
            }
        }

        private void HandleChangeMoney()
        {
            float value = (totalMoney - firstMoney);
            UpdateSlider(value / maxValue);
        }

        private void SetSliderMaxValue(int value)
        {
           maxValue = value;
        }
    }
}
