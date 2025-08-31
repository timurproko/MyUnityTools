using UnityEngine;

public class Events2_Enemy : MonoBehaviour
{
    [ContextMenu("Kill Enemy")]
    private void KillEnemy()
    {
        Events2_EventManager.SendEnemyKilled();
    }
}