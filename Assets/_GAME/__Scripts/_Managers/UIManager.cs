using System;
using DG.Tweening;
using Rentire.Core;
using UnityEngine;

public class UIManager : Singleton<UIManager>,IGameStateObserver
{
    [SerializeField] private GameObject moneyHolder;
    private void Awake()
    {
        AddToGameObserverList();
    }
    

    #region Level Buttons

    public void Button_NextLevel()
    {
        LevelManager.Instance.NextLevel();
    }

    public void Button_RestartLevel()
    {
        LevelManager.Instance.RestartLevel();
    }

    #endregion


    public void AddToGameObserverList()
    {
        gameManager.AddListener(this);
    }

    public void OnGameStateChanged()
    {
        
    }

    public void PunchMoneyHolder()
    {
        moneyHolder.transform.DOPunchScale(0.3f * Vector3.one, 0.2f, 0, 0.2f)
            .OnComplete(() => moneyHolder.transform.DOScale(Vector3.one, 0));
    }
}
