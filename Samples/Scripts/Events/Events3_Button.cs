using UnityEngine;
using UnityEngine.Events;

public class Events3_Button : MonoBehaviour
{
    public UnityEvent _onPressed;

    [ContextMenu("Open Door")]
    private void OpenDoor()
    {
        _onPressed.Invoke();
    }
}