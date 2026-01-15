using System;
using UnityEngine;

public static class EventsManager
{
    //Here is all events which can be called
    //IMPORTANT! Events do not check weather gameObject is active or not! They
    //will call function even in the disabled gameObjects!!!!
    public static event Action<Vector2> OnRobotsActivate;
    public static event Action OnAllRobotsDeactivate;
    public static event Action<GameObject> OnDamageTaken;
    public static event Action OnWeaponSwitched;
    public static event Action OnStartFire;
    public static event Action OnFireBullet;
    public static event Action OnStopFire;
    public static event Action OnNewWeaponAcquired;

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

    public static void CallOnWeaponSwitched()
    {
        OnWeaponSwitched?.Invoke();
    }

    public static void CallOnStartFire()
    {
        OnStartFire?.Invoke();
    }

    public static void CallOnStopFire()
    {
        OnStopFire?.Invoke();
    }

    public static void CallOnFireBullet()
    {
        OnFireBullet?.Invoke();
    }

    public static void CallOnNewWeaponAcquired()
    {
        OnNewWeaponAcquired?.Invoke();
    }
}
