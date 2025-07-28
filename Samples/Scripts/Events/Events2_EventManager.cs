using UnityEngine.Events;

public class Events2_EventManager
{
    // UnityEvent instance is not null when it’s declared, but
    // You need to initialize UnityEvent with new().
    // Even if no listeners are added, the UnityEvent instance is still valid and can be invoked.
    public static UnityEvent OnEnemyKilled = new();

    public static void SendEnemyKilled()
    {
        OnEnemyKilled.Invoke();
    }
}