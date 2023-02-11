using _GAME.__Scripts.Ui;
using Rentire.Utils;
using TMPro;
using UnityEngine;

namespace _GAME.__Scripts.Incremental
{
    public abstract class IncrementalBase : MonoBehaviour
    {
        public TMP_Text incrementalTextMoney;
        public IncrementalSO incrementalSo;

        public float requiredMoney;
        private void Start()
        {
            SetIncrementalText();
        }
        
        public bool RequireMoney()
        {
            GetRequiredMoney();

            if (UserPrefs.GetTotalMoney() >= incrementalSo.requiredMoney)
            {
                UserPrefs.SetTotalMoney((int)(UserPrefs.GetTotalMoney() - requiredMoney));
                MoneyManager.Instance.SetMoneyText();
                
                requiredMoney += requiredMoney * incrementalSo.multipleIncrease + incrementalSo.plusValue;

                SetRequiredMoney();
                
                SetIncrementalText();

                return true;
            }


            return false;
        }
        
        protected abstract void SetIncrementalText();

        protected abstract void SetRequiredMoney();
        
        protected abstract void GetRequiredMoney();

    }
}