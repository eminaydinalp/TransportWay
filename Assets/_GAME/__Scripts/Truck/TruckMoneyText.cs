using System;
using DG.Tweening;
using Doozy.Engine.Extensions;
using TMPro;
using UnityEngine;

namespace _GAME.__Scripts.Truck
{
    public class TruckMoneyText : MonoBehaviour
    {

        public TruckController truckController;

        private void Awake()
        {
            truckController = GetComponentInParent<TruckController>();
        }

        public void OpenMoneyText(int moneyCount , Vector3 pos)
        {
            var moneyText = PoolManager.Instance.Spawn_Object(PoolsEnum.MoneyText , pos + (Vector3.up * 2) , Quaternion.identity , 1.2f);
            moneyText.GetComponentInChildren<TMP_Text>().text = "+ " + moneyCount + "$";
            moneyText.GetComponentInChildren<TMP_Text>().DOFade(1, 0);
            UIManager.Instance.PunchMoneyHolder();
            moneyText.transform.DOScale(Vector3.one * 0.3f, 0.4f).From(0);
            moneyText.transform.DOMoveY(moneyText.transform.position.y + 5, 1f);
            moneyText.GetComponentInChildren<TMP_Text>().DOFade(0, 1f);
        }
    }
}