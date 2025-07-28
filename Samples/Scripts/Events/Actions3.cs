using System;
using UnityEngine;

// Invoke Action as methods Parameter
public class Actions3 : MonoBehaviour
{
    private void Start()
    {
        DoSomething();
        DoSomething(CloseDoor);
    }

    private void DoSomething(Action callback = null)
    {
        OpenDoor();
        callback?.Invoke();
    }

    private void CloseDoor()
    {
        Debug.Log("I have closed the door");
    }
    
    private void OpenDoor()
    {
        Debug.Log("I have opened the door");
    }
}