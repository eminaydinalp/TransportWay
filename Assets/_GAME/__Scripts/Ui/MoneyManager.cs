using System;
using DG.Tweening;
using Rentire.Core;
using Rentire.Utils;
using TMPro;
using UnityEngine;

namespace _GAME.__Scripts.Ui
{
    public class MoneyManager : Singleton<MoneyManager>
    {
        public TMP_Text moneyCount;
        public int defaultMoney;
        public int trickMoney;
        
        private void Start()
        {
            moneyCount.text = UserPrefs.GetTotalMoney().ToString("0");
        }

        public void SetMoneyText()
        {
            moneyCount.text = UserPrefs.GetTotalMoney().ToString();
        }

        public void IncreaseMoney(int amount)
        {
            UserPrefs.IncreaseMoney(amount);
            SetMoneyText();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                UserPrefs.IncreaseMoney(trickMoney);
                SetMoneyText();
            }
        }
    }
}
