using System;
using DG.Tweening;
using Doozy.Engine.Extensions;
using TMPro;
using UnityEngine;

namespace _GAME.__Scripts.Truck
{
    public class FullText: MonoBehaviour
    {
        public TMP_Text moneyText;

        private Camera _camera;
        private void Awake()
        {
            _camera = Camera.main;
        }

        public void OpenMoneyText()
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.zero;
            
            moneyText.color = moneyText.color.WithAlpha(1f);
            moneyText.color = Color.red;
            transform.DOScale(Vector3.one*0.5f, 0.4f);
            transform.DOLocalMoveY(2f, 0.5f);
            moneyText.DOColor(moneyText.color.WithAlpha(0), 0.5f).SetDelay(0.5f);
        }
    }
}