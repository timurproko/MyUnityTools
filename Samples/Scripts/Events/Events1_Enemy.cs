using UnityEngine;

public class Events1_Enemy : MonoBehaviour
{
    [ContextMenu("Kill Enemy")]
    private void KillEnemy()
    {
        Events1_EventManager.SendEnemyKilled();
    }
}