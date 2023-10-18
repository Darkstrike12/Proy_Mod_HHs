using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/New Event")]
public class GameEvent : ScriptableObject
{
    List<GameEventListener> eventListeners = new List<GameEventListener>();

    public void TriggerEvent()
    {
        foreach (GameEventListener listener in eventListeners)
        {
            listener.OnEventTriggered();
        }
    }

    public void AddListener(GameEventListener listener)
    {
        eventListeners.Add(listener);
    }

    public void RemoveListener(GameEventListener listener)
    {
        eventListeners.Remove(listener);
    }
}
