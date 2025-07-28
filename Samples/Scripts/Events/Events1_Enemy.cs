using Sirenix.OdinInspector;
using UnityEngine;

public class Events1_Enemy : MonoBehaviour
{

    [Button]
    private void KillEnemy()
    {
        Events1_EventManager.SendEnemyKilled();
    }

}