using System;
using System.Collections;
using System.Collections.Generic;
using Rentire.Core;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    #region Events / Delegates
    
    public delegate void ScoreUpdate(int score);
    public event ScoreUpdate Event_ScoreUpdated;
    
    #endregion
    
    #region Methods

    public void Invoke_ScoreUpdate(int score)
    {
        if (Event_ScoreUpdated != null)
            Event_ScoreUpdated(score);
    }

    #endregion

    #region Collection Updater

    public delegate void CollectionUpdated();

    public event CollectionUpdated event_CollectionUpdated;

    public void Invoke_CollectionUpdated()
    {
        event_CollectionUpdated?.Invoke();
    }

    #endregion


    public static event UnityAction OnSplineReset;

    public void InvokeOnSplineReset()
    {
        OnSplineReset?.Invoke();
    }

    public static event UnityAction OnAddNewTruck;

    public void InvokeOnAddNewTruck()
    {
        OnAddNewTruck?.Invoke();
    }
    
    public static event UnityAction OnMergeButtonClick;

    public void InvokeOnMergeButtonClick()
    {
        OnMergeButtonClick?.Invoke();
    }
    public static event UnityAction OnMergeFinish;

    public void InvokeOnMergeFinish()
    {
        OnMergeFinish?.Invoke();
    }
    
    public static event UnityAction OnClickSpeed;

    public void InvokeOnClickSpeed()
    {
        OnClickSpeed?.Invoke();
    }
    
    public static event UnityAction OnEndHome;

    public void InvokeOnEndHome()
    {
        OnEndHome?.Invoke();
    }
    
    public static event UnityAction OnChangeTruckCapacity;

    public void InvokeOnChangeTruckCapacity()
    {
        OnChangeTruckCapacity?.Invoke();
    }
}
