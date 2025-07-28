using System;

public class Events1_EventManager
{
    public static Action OnEnemyKilled;

    // The event keyword provides controlled access to the delegate.
    // External code can only subscribe to or unsubscribe from the event, but cannot directly set or clear the delegate 
    // public static event Action OnEnemyKilled;
    
    public static void SendEnemyKilled()
    {
        OnEnemyKilled?.Invoke();
    }
}