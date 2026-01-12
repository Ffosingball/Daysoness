using System;
using UnityEngine;

public static class EventsManager
{
    //Here is all events which can be called
    public static event Action<Vector2> OnRobotsActivate;
    public static event Action OnAllRobotsDeactivate;
    public static event Action<GameObject> OnDamageTaken;

    //Event calls
    public static void CallOnRobotsActivate(Vector2 callerPosition)
    {
        OnRobotsActivate?.Invoke(callerPosition);
    }

    public static void CallOnDamageTaken(GameObject gameObject)
    {
        OnDamageTaken?.Invoke(gameObject);
    }

    public static void CallOnAllRobotsDeactivate()
    {
        OnAllRobotsDeactivate?.Invoke();
    }
}
