using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Events3_Button : MonoBehaviour
{
    public UnityEvent _onPressed;
    
    [Button]
    private void OpenDoor()
    {
        _onPressed.Invoke();
    } 
}