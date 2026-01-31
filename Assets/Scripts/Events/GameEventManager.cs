using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager
{
    private static GameEventManager _instance;
    public static GameEventManager Instance => _instance ??= new GameEventManager();

    private Dictionary<string, Action<GameEvent>> eventListeners = new();

    public void Subscribe(string eventName, Action<GameEvent> listener)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = delegate { };
        }

        eventListeners[eventName] += listener;
    }

    public void Unsubscribe(string eventName, Action<GameEvent> listener)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] -= listener;
            if (eventListeners[eventName] == null)
            {
                eventListeners.Remove(eventName);
            }
        }
    }

    public void TriggerEvent(GameEvent gameEvent)
    {
        if (eventListeners.TryGetValue(gameEvent.EventName, out var listeners))
        {
            //Debug.Log("[EVENT] " + gameEvent.EventName);
            listeners?.Invoke(gameEvent);
        }
    }
}