using System.Collections;
using System.Collections.Generic;
using Rentire.Core;
using Sirenix.OdinInspector;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    private readonly ObservedValue<GameState> GameStatus = new ObservedValue<GameState>(GameState.WaitingToStart);
    public GameState CurrentGameState = GameState.WaitingToStart;
    public IList<IGameStateObserver> gameStateObserverList;

    public GameObject clickSpeed;

    private void Start()
    {
        CurrentGameState = GameState.WaitingToStart;
    }

    public void AddListener(IGameStateObserver gameStateObserver)
    {
        gameStateObserverList ??=new List<IGameStateObserver>();
        if(!gameStateObserverList.Contains(gameStateObserver))
            gameStateObserverList.Add(gameStateObserver);
    }

    /// <summary>
    /// Change game state to Running
    /// </summary>
    public void SetGameRunning()
    {
        SetGameState(GameState.Running);
    }

    /// <summary>
    /// Change Game state to Success
    /// </summary>
    public void SetGameSuccess()
    {
        // if(gameState != GameState.Final)
        // {
        //     return;
        // }

        SetGameState(GameState.Success);

    }
    /// <summary>
    /// Change Game state to Final
    /// </summary>
    public void SetGameFinal()
    {
        if (gameState != GameState.Running)
            return;
        SetGameState(GameState.Final);
    }

    /// <summary>
    /// Change Game state to Fail
    /// </summary>
    public void SetGameFail()
    {
        if (gameState != GameState.Running)
        {
            return;
        }

        SetGameState(GameState.Fail);
    }

    public void SetGameState(GameState gameState)
    {
        GameStatus.Value = gameState;

        Log.Info("Current game state is set to " + gameState);
    }


    private void GameStateChanged()
    {
        CurrentGameState = GameStatus.Value;
        if(gameStateObserverList != null)
            for (int i = 0; i < gameStateObserverList.Count; i++)
            {
                if(gameStateObserverList[i] != null)
                    gameStateObserverList[i].OnGameStateChanged();
            }
        if (CurrentGameState == GameState.Success)
        {
            LevelManager.Instance.IncreaseLevelNo();

        }
    }

    private void OnEnable()
    {
        GameStatus.OnValueChange += GameStateChanged;
        EventManager.OnEndHome += ClickSpeedOpen;
    }

    private void OnDisable()
    {
        GameStatus.OnValueChange -= GameStateChanged;
        EventManager.OnEndHome -= ClickSpeedOpen;
    }

    private void ClickSpeedOpen()
    {
        clickSpeed.SetActive(true);
    }

}
