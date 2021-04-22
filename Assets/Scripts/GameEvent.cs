using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();

    [HideInInspector]
    public bool boolValue;
    [HideInInspector]
    public string stringValue;
    [HideInInspector]
    public int intValue;
    [HideInInspector]
    public float floatValue;
    [HideInInspector]
    public Vector2 vectorValue;

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void Raise(bool _boolValue)
    {
        boolValue = _boolValue;

        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void Raise(string _stringValue)
    {
        stringValue = _stringValue;

        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void Raise(int _intValue)
    {
        intValue = _intValue;

        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void Raise(Vector2 _vectorValue)
    {
        vectorValue = _vectorValue;

        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void Raise(bool _boolValue, float _floatValue, Vector2 _vectorValue)
    {
        boolValue = _boolValue;
        floatValue = _floatValue;
        vectorValue = _vectorValue;

        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void Raise(string _stringValue, int _intValue)
    {
        stringValue = _stringValue;
        intValue = _intValue;

        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void Raise(string _stringValue, int _intValue, Vector2 _vectorValue)
    {
        stringValue = _stringValue;
        intValue = _intValue;
        vectorValue = _vectorValue;

        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}
