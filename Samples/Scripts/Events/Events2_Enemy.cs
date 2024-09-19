using Sirenix.OdinInspector;
using UnityEngine;

public class Events2_Enemy : MonoBehaviour
{

    [Button]
    private void KillEnemy()
    {
        Events2_EventManager.SendEnemyKilled();
    }

}