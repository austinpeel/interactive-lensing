using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent Response;

    private void OnEnable()
    {
        if (Event != null)
        {
            Event.RegisterListener(this);
        }
        else
        {
            Debug.Log("Warning : null Event of " + transform.name + " transform.");
        }
    }

    private void OnDisable()
    {
        if (Event != null)
        {
            Event.UnregisterListener(this);
        }
        else
        {
            Debug.Log("Warning : null Event of " + transform.name + " transform.");
        }
    }

    public void OnEventRaised()
    {
        if (Response != null)
        {
            Response.Invoke();
        }
        else
        {
            Debug.Log("Warning : null Response of " + transform.name + " transform.");
        }
    }
}
